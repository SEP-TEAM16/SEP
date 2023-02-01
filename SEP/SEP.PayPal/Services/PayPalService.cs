using Newtonsoft.Json;
using PayPal.Api;
using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PayPal.Infrastructure;
using SEP.PayPal.Interfaces;
using SEP.PayPal.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using Payment = PayPal.Api.Payment;

namespace SEP.PayPal.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly ILogger<PayPalService> _logger;
        private readonly PayPalDbContext _payPalDbContext;
        private string CLIENT_ID { get; set; }
        private string CLIENT_SECRET { get; set; }
        private string MODE { get; set; }

        public PayPalService(ILogger<PayPalService> logger, PayPalDbContext payPalDbContext)
        {
            _logger = logger;
            _payPalDbContext = payPalDbContext;

            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            CLIENT_ID = appSettings.GetValue<string>("Secrets:CLIENT_ID");
            CLIENT_SECRET = appSettings.GetValue<string>("Secrets:CLIENT_SECRET");
            MODE = appSettings.GetValue<string>("Secrets:MODE");
            payPalDbContext.PayPalPayment.RemoveRange(payPalDbContext.PayPalPayment.ToList());
            payPalDbContext.SaveChanges();
        }

        public string GetApprovalLink(PayPalPayment payPalPayment)
        {
            try
            {
                payPalPayment.Date = DateTime.Now;
                payPalPayment.Currency = "USD";
                return AuthorizePayment(payPalPayment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public string Subscribe(PayPalPayment payPalPayment)
        {
            using (var client = new HttpClient())
            {
                payPalPayment.Date = DateTime.Now;
                payPalPayment.Date = payPalPayment.Date.AddDays(1);
                string authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(CLIENT_ID + ":" + CLIENT_SECRET));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                //string requestBody = "{\"name\":\""+payPalPayment.Name+"\",\"description\":\""+payPalPayment.Description+"\",\"start_date\":\""+payPalPayment.Date.ToString("s")+"Z"+ "\",\"plan\":{\"id\":\"P-4VX409764F671362VMPMSKEY\"},\"payer\":{\"payment_method\":\"paypal\"},\"override_merchant_preferences\": {\"return_url\":\"https://localhost:5050/paypal/continue\",\"cancel_url\":\"https://localhost:5050/paypal/cancel\"}}";
                //string requestBody2 = "{\"product_id\":\"PROD-9R10736519993944T\",\"name\":\"" + payPalPayment.Name + "\",\"billing_cycles\":[{\"tenure_type\":\"REGULAR\", \"sequence\":1, \"frequency\":{\"interval_unit\":\"MONTH\"}, \"pricing_scheme\": {\"fixed_price\": {\"value\": \"50\",\"currency_code\": \"USD\"}}}],\"payment_preferences\":{\"payment_preferences\":true}}";
                //string requestBody3 = "{\"name\":\"" + payPalPayment.Name + "\",\"type\":\"SERVICE\"}";
                string requestBody = "{\"plan_id\": \"P-4VX409764F671362VMPMSKEY\",\r\n    \"start_time\": \"" + payPalPayment.Date.ToString("s") + "Z" + "\",\r\n    \"shipping_amount\": {\r\n        \"currency_code\": \"USD\",\r\n        \"value\": \""+ payPalPayment.Amount.ToString("0.00").Replace(',', '.')+ "\"\r\n    },\r\n    \"subscriber\": {\r\n        \"name\": {\r\n            \"given_name\": \"" + payPalPayment.FirstName + "\",\r\n            \"surname\": \"" + payPalPayment.LastName + "\"\r\n        },\r\n        \"email_address\": \"" + payPalPayment.Email + "\",\r\n        \"shipping_address\": {\r\n            \"name\": {\r\n                \"full_name\": \"" + payPalPayment.FirstName + " " + payPalPayment.LastName + "\"\r\n            },\r\n            \"address\": {\r\n                \"address_line_1\": \"2211 N First Street\",\r\n                \"address_line_2\": \"Building 17\",\r\n                \"admin_area_2\": \"San Jose\",\r\n                \"admin_area_1\": \"CA\",\r\n                \"postal_code\": \"95131\",\r\n                \"country_code\": \"US\"\r\n            }\r\n        }\r\n    },\r\n    \"application_context\": {\r\n        \"brand_name\": \"Example Inc\",\r\n        \"locale\": \"en-US\",\r\n        \"shipping_preference\": \"SET_PROVIDED_ADDRESS\",\r\n        \"user_action\": \"SUBSCRIBE_NOW\",\r\n        \"payment_method\": {\r\n            \"payer_selected\": \"PAYPAL\",\r\n            \"payee_preferred\": \"IMMEDIATE_PAYMENT_REQUIRED\"\r\n        },\r\n        \"return_url\": \"https://localhost:5050/paypal/continue-sub\",\r\n        \"cancel_url\": \"https://localhost:5050/paypal/cancel-sub\"\r\n    }}";
                /*
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var result = client.PostAsync("https://api.sandbox.paypal.com/v1/payments/billing-agreements", content).Result;
                var responseContent = result.Content.ReadAsStringAsync().Result;*/
                var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.sandbox.paypal.com/v1/billing/subscriptions");
                //var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.sandbox.paypal.com/v1/billing/plans");
                //var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.sandbox.paypal.com/v1/catalogs/products");
                //var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.sandbox.paypal.com/v1/payments/billing-plans/4VX409764F671362VMPMSKEY");
                httpRequest.Method = "POST";
                //httpRequest.Method = "GET";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers.Add("Authorization:" + new AuthenticationHeaderValue("Basic", authHeaderValue));
                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(requestBody);
                //streamWriter.Write(requestBody2);
                //streamWriter.Write(requestBody3);
                streamWriter.Close();

                var getdata = new Agreement();
                using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
                using (var stream = webresponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    getdata = JsonConvert.DeserializeObject<Agreement>(json);
                }
                var url = getdata.links.FirstOrDefault(l => l.rel.Equals("approve")).href;
                payPalPayment.Token = url.Split("=")[1];
                _payPalDbContext.PayPalPayment.Add(payPalPayment);
                _payPalDbContext.SaveChanges();
                return url;
            }
        }

        public PayPalPayment Pay2(string subscription_id, string ba_token, string token)
        {
            var payPalPayment = _payPalDbContext.PayPalPayment.FirstOrDefault(x => x.Token == ba_token);
            if (payPalPayment != null)
            {
                try
                {
                    payPalPayment.PaymentApproval = PaymentApprovalType.Success;
                    _payPalDbContext.SaveChanges();
                    return payPalPayment;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    payPalPayment.PaymentApproval = PaymentApprovalType.Rejected;
                    _payPalDbContext.SaveChanges();
                    return payPalPayment;
                }
            }
            return null;
        }
        public PayPalPayment Pay(string paymentId, string payerId, string token)
        {
            var payPalPayment = _payPalDbContext.PayPalPayment.FirstOrDefault(x => x.Token == token);
            if (payPalPayment != null)
            {
                try
                {
                    var payment = ExecutePayment(paymentId, payerId);
                    payPalPayment.PaymentApproval = PaymentApprovalType.Success;
                    _payPalDbContext.SaveChanges();
                    return payPalPayment;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    payPalPayment.PaymentApproval = PaymentApprovalType.Rejected;
                    _payPalDbContext.SaveChanges();
                    return payPalPayment;
                }
            }
            return null;
        }

        public PayPalPayment Cancel(string token)
        {
            var payPalPayment = _payPalDbContext.PayPalPayment.FirstOrDefault(x => x.Token == token);
            payPalPayment.PaymentApproval = PaymentApprovalType.Canceled;
            _payPalDbContext.SaveChanges();
            return payPalPayment;
        }
        private string AuthorizePayment(PayPalPayment payPalPayment)
        {
            var payer = new Payer
            {
                payment_method = "paypal"
            };
            var payerInfo = new PayerInfo();
            if (string.IsNullOrEmpty(payPalPayment.Name))
            {
                payerInfo.first_name = payPalPayment.FirstName;
                payerInfo.last_name = payPalPayment.LastName;
            }
            else
            {
                payerInfo.first_name = payPalPayment.Name;
                payerInfo.last_name = payPalPayment.Name;
            }
            payerInfo.email = payPalPayment.Email;

            payer.payer_info = payerInfo;

            var redirectUrls = GetRedirectURLs();
            var listTransaction = GetTransactionInformation(payPalPayment);
            var requestPayment = new Payment()
            {
                transactions = listTransaction,
                redirect_urls = redirectUrls,
                payer = payer,
                intent = "authorize"
            };

            var apiContext = new APIContext(GetAccessToken());
            var approvedPayment = requestPayment.Create(apiContext);
            payPalPayment.PaymentApproval = PaymentApprovalType.Pending;
            payPalPayment.Token = approvedPayment.GetTokenFromApprovalUrl();
            _payPalDbContext.PayPalPayment.Add(payPalPayment);
            _payPalDbContext.SaveChanges();
            return FindApprovalLink(approvedPayment);
        }

        private string GetAccessToken()
        {
            var config = ConfigManager.Instance.GetProperties();
            config.Add("clientId", CLIENT_ID);
            config.Add("clientSecret", CLIENT_SECRET);
            config.Add("mode", MODE);
            return new OAuthTokenCredential(config).GetAccessToken();
        }
        private List<Transaction> GetTransactionInformation(PayPalPayment payPalPayment)
        {
            var details = new Details
            {
                shipping = "0.00",
                subtotal = payPalPayment.Amount.ToString("0.00").Replace(',', '.'),
                tax = "0.00"
            };
            var amount = new Amount
            {
                currency = payPalPayment.Currency,
                total = payPalPayment.Amount.ToString("0.00").Replace(',', '.'),
                details = details
            };
            var transaction = new Transaction
            {
                amount = amount,
                description = payPalPayment.Description
            };
            
            var item = new Item
            {
                currency = payPalPayment.Currency,
                name = payPalPayment.ItemName,
                price = payPalPayment.Amount.ToString("0.00").Replace(',', '.'),
                tax = "0.00",
                quantity = "1"
            };
            var items = new List<Item>
            {
                item
            };
            var itemList = new ItemList
            {
                items = items
            };
            transaction.item_list = itemList;

            var listTransaction = new List<Transaction>
            {
                transaction
            };

            return listTransaction;
        }
        private string FindApprovalLink(Payment approvedPayment)
        {
            var links = approvedPayment.links;
            String approvalLink = null;
            foreach (var link in links)
            {
                if (link.rel.ToLower().Equals("approval_url"))
                {
                    approvalLink = link.href;
                    break;
                }
            }
            return approvalLink;
        }
        private Payment ExecutePayment(string paymentId, string payerId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            var payment = new Payment
            {
                id = paymentId
            };
            var apiContext = new APIContext(GetAccessToken());
            return payment.Execute(apiContext, paymentExecution);
        }

        private RedirectUrls GetRedirectURLs()
        {
            var redirectUrls = new RedirectUrls
            {
                cancel_url = "https://localhost:5050/paypal/cancel",
                return_url = "https://localhost:5050/paypal/continue"
            };
            return redirectUrls;
        }
    }
}
