﻿using AutoMapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;
using Nancy.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SEP.Common.DTO;
using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.DTO;
using SEP.PSP.Infrastructure;
using SEP.PSP.Interfaces;
using SEP.PSP.Models;
using System.Net;
using System.Text;
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

           /* var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("makePayment", exclusive: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                MakePayPalPayment(JsonSerializer.Deserialize<PSPPaymentDTO>(message));
            };

            channel.BasicConsume(queue: "makePayment", autoAck: true, consumer: consumer);
            Console.ReadKey();*/
        }

        public string MakePayPalSubscribe(PSPPaymentDTO pspPaymentDto)
        {
            var merchant = _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Key.Equals(pspPaymentDto.Key));
            if (merchant is null)
                return null;

            var subscribedServices = _PSPDbContext.Subscriptions.Where(sub => sub.Merchant.Id.Equals(merchant.Id)).Select(sub => sub.PaymentMicroserviceType);
            if (!subscribedServices.Contains(PaymentMicroserviceType.Paypal))
                return null;

            var authKey = AuthKeys.FirstOrDefault(a => a.PaymentMicroserviceType.Equals(PaymentMicroserviceType.Paypal) && !a.Route.Contains("subscribe"));
            var payPalPayment = _mapper.Map<PSPPayment>(pspPaymentDto);
            var authToken = GetBearerToken(PaymentMicroserviceType.Paypal);

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/" + authKey.Route +"/subscribe");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken.Token}");
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            var pspPayPalPaymentDto = payPalPayment.ConvertToPSPPayPalPaymentDTO();
            pspPayPalPaymentDto.MerchantId = merchant.Id.ToString();
            streamWriter.Write(JsonSerializer.Serialize(pspPayPalPaymentDto));
            streamWriter.Close();

            var getdata = string.Empty;
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
        public string MakePayPalPayment(PSPPaymentDTO pspPaymentDto)
        {
            var merchant = _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Key.Equals(pspPaymentDto.Key));
            if (merchant is null)
                return null;

            var subscribedServices = _PSPDbContext.Subscriptions.Where(sub => sub.Merchant.Id.Equals(merchant.Id)).Select(sub => sub.PaymentMicroserviceType);
            if (!subscribedServices.Contains(PaymentMicroserviceType.Paypal))
                return null;

            var authKey = AuthKeys.FirstOrDefault(a => a.PaymentMicroserviceType.Equals(PaymentMicroserviceType.Paypal));
            var payPalPayment = _mapper.Map<PSPPayment>(pspPaymentDto);
            var authToken = GetBearerToken(PaymentMicroserviceType.Paypal);

            var httpRequest = (HttpWebRequest) HttpWebRequest.Create("https://localhost:5050/" + authKey.Route);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken.Token}");
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            var pspPayPalPaymentDto = payPalPayment.ConvertToPSPPayPalPaymentDTO();
            pspPayPalPaymentDto.MerchantId = merchant.Id.ToString();
            streamWriter.Write(JsonSerializer.Serialize(pspPayPalPaymentDto));
            streamWriter.Close();

            var getdata = string.Empty;
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
        public void MakeBitcoinPayment(PSPBitcoinPaymentDTOForReceive pspBitcoinPaymentDtoo)
        {
            var merchant = _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Key.Equals(pspBitcoinPaymentDtoo.Key));
            if (merchant is null)
                return;

            var subscribedServices = _PSPDbContext.Subscriptions.Where(sub => sub.Merchant.Id.Equals(merchant.Id)).Select(sub => sub.PaymentMicroserviceType);
            if (!subscribedServices.Contains(PaymentMicroserviceType.Bitcoin))
                return;

            var authKey = AuthKeys.FirstOrDefault(a => a.PaymentMicroserviceType.Equals(PaymentMicroserviceType.Bitcoin));
            var bitcoinPayment = _mapper.Map<PSPPayment>(pspBitcoinPaymentDtoo);
            var authToken = GetBearerToken(PaymentMicroserviceType.Bitcoin);

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/" + authKey.Route);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken.Token}");
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            var pspBitcoinPaymentDto = bitcoinPayment.ConvertToPSPBitcoinPaymentDTO();
            pspBitcoinPaymentDto.MerchantId = merchant.Id.ToString();
            pspBitcoinPaymentDto.PublicKey = pspBitcoinPaymentDtoo.PublicKey;
            pspBitcoinPaymentDto.PrivateKey = pspBitcoinPaymentDtoo.PrivateKey;
            streamWriter.Write(JsonSerializer.Serialize(pspBitcoinPaymentDto));
            streamWriter.Close();

            var getdata = string.Empty;
            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            var jss = new JavaScriptSerializer();
            PSPBitcoinPaymentDTO fromBack = jss.Deserialize<PSPBitcoinPaymentDTO>(getdata);
            bitcoinPayment.PaymentApproval = fromBack.PaymentApproval;
            bitcoinPayment.Date = DateTime.Now;
            bitcoinPayment.Currency = fromBack.Currency;
            bitcoinPayment.Merchant = merchant;
            _PSPDbContext.PSPPayments.Add(bitcoinPayment);
            _PSPDbContext.SaveChanges();
            return;
        }
        public string MakeBankPayment(PSPPaymentDTO pspPaymentDto)
        {
            var merchant = _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Key.Equals(pspPaymentDto.Key));
            if (merchant is null)
                return null;

            var subscribedServices = _PSPDbContext.Subscriptions.Where(sub => sub.Merchant.Id.Equals(merchant.Id)).Select(sub => sub.PaymentMicroserviceType);
            if (!subscribedServices.Contains(PaymentMicroserviceType.Card))
                return null;

            var authKey = AuthKeys.FirstOrDefault(a => a.PaymentMicroserviceType.Equals(PaymentMicroserviceType.Card) && !a.Route.Contains("2"));
            var bankPayment = _mapper.Map<PSPPayment>(pspPaymentDto);
            var authToken = GetBearerToken(PaymentMicroserviceType.Card);

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/" + authKey.Route);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken.Token}");
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            var pspBankPaymentDto = bankPayment.ConvertToPSPBankPaymentDTO();
            pspBankPaymentDto.MerchantId = merchant.Id.ToString();
            streamWriter.Write(JsonSerializer.Serialize(pspBankPaymentDto));
            streamWriter.Close();

            var getdata = string.Empty;
            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            bankPayment.PaymentApproval = PaymentApprovalType.Pending;
            bankPayment.Date = DateTime.Now;
            bankPayment.Currency = "USD";
            bankPayment.Merchant = merchant;
            _PSPDbContext.PSPPayments.Add(bankPayment);
            _PSPDbContext.SaveChanges();
            return getdata;
        }
        public string MakeQRPayment(PSPPaymentDTO pspPaymentDto)
        {
            var merchant = _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Key.Equals(pspPaymentDto.Key));
            if (merchant is null)
                return null;
            
            var subscribedServices = _PSPDbContext.Subscriptions.Where(sub => sub.Merchant.Id.Equals(merchant.Id)).Select(sub => sub.PaymentMicroserviceType);
            if (!subscribedServices.Contains(PaymentMicroserviceType.QR))
                return null;

            var bankPayment = _mapper.Map<PSPPayment>(pspPaymentDto);
            var authToken = GetBearerToken(PaymentMicroserviceType.QR);
            var authKey = AuthKeys.FirstOrDefault(a => a.PaymentMicroserviceType.Equals(PaymentMicroserviceType.QR) && !a.Route.Contains("2"));

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/" + authKey.Route);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken.Token}");
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            var pspBankPaymentDto = bankPayment.ConvertToPSPBankPaymentDTO();
            pspBankPaymentDto.MerchantId = merchant.Id.ToString();
            streamWriter.Write(JsonSerializer.Serialize(pspBankPaymentDto));
            streamWriter.Close();

            var getdata = string.Empty;
            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            bankPayment.PaymentApproval = PaymentApprovalType.Pending;
            bankPayment.Date = DateTime.Now;
            bankPayment.Currency = "USD";
            bankPayment.Merchant = merchant;
            _PSPDbContext.PSPPayments.Add(bankPayment);
            _PSPDbContext.SaveChanges();
            return getdata;
        }

        public void UpdatePayment(PSPPayment PSPPayment)
        {
            var payPalPayment = _PSPDbContext.PSPPayments.FirstOrDefault(payment => payment.IdentityToken.Equals(PSPPayment.IdentityToken));

            if (payPalPayment is null)
            {
                _logger.LogWarning("Payment could not be found!");
                return;
            }

            payPalPayment.PaymentApproval = PSPPayment.PaymentApproval;

            _PSPDbContext.SaveChanges();
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

            try
            {
                var jss = new JavaScriptSerializer();
                var getdata = string.Empty;
                var httpRequest = PSPPayment.PaymentApproval == PaymentApprovalType.Success 
                    ? (HttpWebRequest)HttpWebRequest.Create("https://localhost:7035/api/payment/continue")
                    : (HttpWebRequest)HttpWebRequest.Create("https://localhost:7035/api/payment/cancel");
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";

                var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
                httpRequest.Headers["key"] = appSettings.GetValue<string>("Secrets:AutorizationKey");

                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(jss.Serialize(new StringDTO(PSPPayment.IdentityToken)));
                streamWriter.Close();

                using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
                using (var stream = webresponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    getdata = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning("Couldn't contact webshop");
            }

        }

        public Subscription SubscribeWebshopToPayment(Subscription subscription)
        {
            var merchant = _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Port.Equals(subscription.Merchant.Port));
            if (merchant == null)
            {
                merchant = new Merchant
                {
                    Port = subscription.Merchant.Port,
                    Key = Guid.NewGuid().ToString()
                };
                subscription.Merchant = merchant;
                _PSPDbContext.Subscriptions.Add(subscription);
            }
            else
            {
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
                    return subscription;
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
        public List<Subscription> GetSubscribedByPort(string port) {
            return _PSPDbContext.Subscriptions.Where(sub => sub.Merchant.Port.Equals(port)).ToList();
        }

        public Boolean RemoveServiceType(string serviceType, string port) {
            List<Subscription> subscriptionsFromDatabase = _PSPDbContext.Subscriptions.Where(sub => sub.Merchant.Port.Equals(port)).ToList();
            foreach (Subscription sub in subscriptionsFromDatabase) {
                if (sub.PaymentMicroserviceType.ToString().Equals(serviceType))
                {
                    subscriptionsFromDatabase.Remove(sub);
                    _PSPDbContext.Subscriptions.Remove(sub);
                    checkMerchants(subscriptionsFromDatabase, port);
                    _PSPDbContext.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        private void checkMerchants(List<Subscription> subscriptions, string port) {
            Boolean exist = false;
            foreach (Subscription sub in subscriptions) {
                if (sub.Merchant.Port.Equals(port)) {
                    exist = true;
                    break;
                }    
            }
            if (!exist)
                RemoveMerchant(port);
        }

        private void RemoveMerchant(string port) {
            List<Merchant> merchantsFromDatabase = _PSPDbContext.Merchants.Where(mer => mer.Port.Equals(port)).ToList();
            foreach (Merchant mer in merchantsFromDatabase) {
                if(mer.Port.Equals(port))
                {
                    _PSPDbContext.Merchants.Remove(mer);
                }
            }
        }

        public Merchant GetMerchantByPort(string port) {
            return _PSPDbContext.Merchants.FirstOrDefault(mer => mer.Port.Equals(port));
        }
    }
}
