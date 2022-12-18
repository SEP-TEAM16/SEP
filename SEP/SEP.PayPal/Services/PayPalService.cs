using PayPal.Api;
using SEP.Common.Enums;
using SEP.PayPal.Infrastructure;
using SEP.PayPal.Interfaces;
using SEP.PayPal.Models;

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
            return false;
        }

        public void Cancel(string token)
        {
            var payPalPayment = _payPalDbContext.PayPalPayment.FirstOrDefault(x => x.Token == token);
            payPalPayment.PaymentApproval = PaymentApprovalType.Canceled;
            _payPalDbContext.SaveChanges();
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
