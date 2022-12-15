using Newtonsoft.Json.Linq;
using PayPal.Api;
using SEP.Common.Enums;
using SEP.PayPal.Infrastructure;
using SEP.PayPal.Interfaces;
using SEP.PayPal.Models;
using System.Globalization;

namespace SEP.PayPal.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly PayPalDbContext payPalDbContext;
        public PayPalService(PayPalDbContext payPalDbContext)
        {
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            CLIENT_ID = appSettings.GetValue<string>("Secrets:CLIENT_ID");
            CLIENT_SECRET = appSettings.GetValue<string>("Secrets:CLIENT_SECRET");
            MODE = appSettings.GetValue<string>("Secrets:MODE");
            this.payPalDbContext = payPalDbContext;
        }
        private string CLIENT_ID { get; set; }
        private string CLIENT_SECRET { get; set; }
        private string MODE { get; set; }

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
                return null;
            }
        }
        public bool Pay(string paymentId, string payerId, string token)
        {
            PayPalPayment payPalPayment = payPalDbContext.PayPalPayment.FirstOrDefault(x => x.Token == token);
            if (payPalPayment != null)
            {
                try
                {
                    ExecutePayment(paymentId, payerId);
                    payPalPayment.PaymentApproval = PaymentApprovalType.Success;
                    payPalDbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    payPalPayment.PaymentApproval = PaymentApprovalType.Rejected;
                    payPalDbContext.SaveChanges();
                    return false;
                }
            }
            return false;
        }

        public void Cancel(string token)
        {
            PayPalPayment payPalPayment = payPalDbContext.PayPalPayment.FirstOrDefault(x => x.Token == token);
            payPalPayment.PaymentApproval = PaymentApprovalType.Canceled;
            payPalDbContext.SaveChanges();
        }
        private string AuthorizePayment(PayPalPayment payPalPayment)
        {
            Payer payer = new Payer();
            payer.payment_method = "paypal";
            PayerInfo payerInfo = new PayerInfo();
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

            RedirectUrls redirectUrls = GetRedirectURLs();
            List<Transaction> listTransaction = GetTransactionInformation(payPalPayment);
            Payment requestPayment = new Payment();
            requestPayment.transactions = listTransaction;
            requestPayment.redirect_urls = redirectUrls;
            requestPayment.payer = payer;
            requestPayment.intent = "authorize";

            APIContext apiContext = new APIContext(GetAccessToken());
            Payment approvedPayment = requestPayment.Create(apiContext);
            payPalPayment.PaymentApproval = PaymentApprovalType.Pending;
            payPalPayment.Token = approvedPayment.GetTokenFromApprovalUrl();
            payPalDbContext.PayPalPayment.Add(payPalPayment);
            payPalDbContext.SaveChanges();
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
            Details details = new Details();
            details.shipping = "0.00";
            details.subtotal = payPalPayment.Amount.ToString("0.00").Replace(',', '.');
            details.tax = "0.00";
            Amount amount = new Amount();
            amount.currency = payPalPayment.Currency;
            amount.total = payPalPayment.Amount.ToString("0.00").Replace(',', '.');
            amount.details = details;
            Transaction transaction = new Transaction();
            transaction.amount = amount;
            transaction.description = payPalPayment.Description;
            ItemList itemList = new ItemList();
            List<Item> items = new List<Item>();
            Item item = new Item();
            item.currency = payPalPayment.Currency;
            item.name = payPalPayment.ItemName;
            item.price = payPalPayment.Amount.ToString("0.00").Replace(',', '.');
            item.tax = "0.00";
            item.quantity = "1";
            items.Add(item);
            itemList.items = items;
            transaction.item_list = itemList;
            List<Transaction> listTransaction = new List<Transaction>();
            listTransaction.Add(transaction);
            return listTransaction;
        }
        private string FindApprovalLink(Payment approvedPayment)
        {
            List<Links> links = approvedPayment.links;
            String approvalLink = null;
            foreach (Links link in links)
            {
                if (link.rel.ToLower().Equals("approval_url"))
                {
                    approvalLink = link.href;
                    break;
                }
            }
            return approvalLink;
        }
        private Payment ExecutePayment(String paymentId, String payerId)
        {
            PaymentExecution paymentExecution = new PaymentExecution();
            paymentExecution.payer_id = payerId;
            Payment payment = new Payment();
            payment.id = paymentId;
            APIContext apiContext = new APIContext(GetAccessToken());
            return payment.Execute(apiContext, paymentExecution);
        }

        private RedirectUrls GetRedirectURLs()
        {
            RedirectUrls redirectUrls = new RedirectUrls();
            redirectUrls.cancel_url = "https://localhost:5050/paypal/cancel";
            redirectUrls.return_url = "https://localhost:5050/paypal/continue";
            return redirectUrls;
        }
    }
}
