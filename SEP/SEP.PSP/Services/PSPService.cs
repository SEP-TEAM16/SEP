﻿using AutoMapper;
using Nancy.Json;
using SEP.Common.DTO;
using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.DTO;
using SEP.PSP.Infrastructure;
using SEP.PSP.Interfaces;
using SEP.PSP.Models;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SEP.PSP.Services
{
    public class PSPService : IPSPService
    {
        private readonly ILogger<PSPService> _logger;
        private readonly PSPDbContext _PSPDbContext;
        private readonly IMapper _mapper;
        private List<AuthKey> AuthKeys;
        public PSPService(ILogger<PSPService> logger, PSPDbContext PSPDbContext, IMapper mapper) {
            _logger = logger;
            _PSPDbContext = PSPDbContext;
            AuthKeys = new List<AuthKey>();
            AuthKeys = GetAuthKeys();
            _mapper = mapper;
            PSPDbContext.Merchants.RemoveRange(PSPDbContext.Merchants.ToList());
            PSPDbContext.Subscriptions.RemoveRange(PSPDbContext.Subscriptions.ToList());
            PSPDbContext.PSPPayments.RemoveRange(PSPDbContext.PSPPayments.ToList());
            PSPDbContext.SaveChanges();
        }

        public string MakePayPalPayment(PSPPaymentDTO pspPaymentDto)
        {
            var merchant = _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Key.Equals(pspPaymentDto.Key));
            if (merchant is null)
                return null;

            var payPalPayment = _mapper.Map<PSPPayment>(pspPaymentDto);
            var authToken = GetBearerToken(PaymentMicroserviceType.Paypal);

            var getdata = string.Empty;
            var jss = new JavaScriptSerializer();
            var authKey = AuthKeys.FirstOrDefault(a => a.PaymentMicroserviceType.Equals(PaymentMicroserviceType.Paypal));

            var httpRequest = (HttpWebRequest) HttpWebRequest.Create("https://localhost:5050/" + authKey.Route);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken.Token}");
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            var pspPayPalPaymentDto = payPalPayment.ConvertToPSPPayPalPaymentDTO();
            pspPayPalPaymentDto.MerchantId = merchant.Id.ToString();
            streamWriter.Write(JsonSerializer.Serialize(pspPayPalPaymentDto));
            streamWriter.Close();

            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            payPalPayment.PaymentApproval = PaymentApprovalType.Pending;
            payPalPayment.Date = DateTime.Now;
            payPalPayment.Currency = "USD";
            payPalPayment.Merchant = merchant;
            _PSPDbContext.PSPPayments.Add(payPalPayment);
            _PSPDbContext.SaveChanges();
            return getdata;
        }

        public void EditPayPalPayment(PSPPayment PSPPayment)
        {
            var payPalPayment = _PSPDbContext.PSPPayments.FirstOrDefault(payment => payment.IdentityToken.Equals(PSPPayment.IdentityToken));
            
            if (payPalPayment is null)
            {
                _logger.LogWarning("Payment could not be found!");
                return;
            }

            payPalPayment.PaymentApproval = PSPPayment.PaymentApproval;
            payPalPayment.Date = PSPPayment.Date;
            payPalPayment.Currency = PSPPayment.Currency;

            _PSPDbContext.SaveChanges();
        }

        public Subscription SubscribeWebshopToPayment(Subscription subscription)
        {
            var merchant = _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Port.Equals(subscription.Merchant.Port));
            if (merchant == null)
            {
                merchant = new Merchant
                {
                    Port = subscription.Merchant.Port,
                    Key = "sadasdnasd" //random kljuc
                };
                subscription.Merchant = merchant;
                _PSPDbContext.Subscriptions.Add(subscription);
            } else {
                List<Subscription> subscriptionsFromDatabase = _PSPDbContext.Subscriptions.Where(sub => sub.Merchant.Port.Equals(subscription.Merchant.Port)).ToList();
                foreach (var sub in subscriptionsFromDatabase)
                {
                    if (sub.PaymentMicroserviceType.Equals(subscription.PaymentMicroserviceType))
                    {
                        return null;
                    }
                }
                subscription.Merchant = merchant;
                _PSPDbContext.Subscriptions.Add(subscription);
            }
            _PSPDbContext.SaveChanges();
            foreach (var aut in AuthKeys)
            {
                if (aut.PaymentMicroserviceType.Equals(subscription.PaymentMicroserviceType))
                {
                    return subscription;
                }
            }
            AuthKeys = GetAuthKeys();
            return subscription;
        }

        private List<AuthKey> GetAuthKeys()
        {
            var getdata = string.Empty;
            var jss = new JavaScriptSerializer();

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5250/authKeys");
            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/json";

            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            httpRequest.Headers["key"] = appSettings.GetValue<string>("Secrets:AutorizationKey");

            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            return jss.Deserialize<List<AuthKey>>(getdata);
        }

        private AuthTokenDTO GetBearerToken(PaymentMicroserviceType paymentMicroserviceType)
        {
            var getdata = string.Empty;
            var jss = new JavaScriptSerializer();
            var authKey = AuthKeys.FirstOrDefault(a => a.PaymentMicroserviceType.Equals(paymentMicroserviceType));

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/auth/" + authKey.Route);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(jss.Serialize(new StringDTO(authKey.Key)));
            streamWriter.Close();

            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            return jss.Deserialize<AuthTokenDTO>(getdata);
        }
    }
}
