using AutoMapper;
using Nancy.Json;
using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.DTO;
using SEP.PSP.Infrastructure;
using SEP.PSP.Interfaces;
using SEP.PSP.Models;
using System.Net;

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
        }

        public PSPPaymentDTO MakePayPalPayment(PSPPaymentDTO PSPPaymentDTO)
        {
            var merchant = _PSPDbContext.Metchants.Where(mer => mer.Key.Equals(PSPPaymentDTO.Key)).First();
            if (merchant == null)
            {
                return null;
            }
            var payPalPayment = _mapper.Map<PSPPayment>(PSPPaymentDTO);
            var authToken = GetBearerToken(PaymentMicroserviceType.Paypal);

            var getdata = string.Empty;
            var jss = new JavaScriptSerializer();
            var authKey = AuthKeys.Where(a => a.PaymentMicroserviceType.Equals(paymentMicroserviceType)).First();

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/" + authKey.Route);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("Authorization", $"Bearer {authToken.Token}");
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(payPalPayment);
            streamWriter.Close();

            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            payPalPayment = jss.Deserialize<PSPPayment>(getdata);
            _PSPDbContext.PSPPayments.Add(payPalPayment);
            _PSPDbContext.SaveChanges();
            return _mapper.Map<PSPPaymentDTO>(payPalPayment);


        }

        public Subscription SubscribeWebshopToPayment(Subscription subscription)
        {
            Merchant merchant = _PSPDbContext.Metchants.Where(mer => mer.Port.Equals(subscription.Merchant.Port)).First();
            if (merchant == null)
            {
                merchant = new Merchant();
                merchant.Port = subscription.Merchant.Port;
                merchant.Key = "sadasdnasd"; //random kljuc
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
            var authKey = AuthKeys.Where(a => a.PaymentMicroserviceType.Equals(paymentMicroserviceType)).First();

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/auth/"+authKey.Route);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(authKey.Key);
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
