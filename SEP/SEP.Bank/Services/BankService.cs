using SEP.Bank.Infrastructure;
using SEP.Bank.Interfaces;
using SEP.Bank.Models;
using SEP.Common.Enums;
using Stripe;

namespace SEP.Bank.Services
{
    public class BankService : IBankService
    {
        private readonly ILogger<BankService> _logger;
        private readonly BankDbContext _bankDbContext;
        private string API_KEY { get; set; }

        public BankService(ILogger<BankService> logger, BankDbContext bankContext)
        {
            _logger = logger;
            _bankDbContext = bankContext;

            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            API_KEY = appSettings.GetValue<string>("Secrets:API_KEY");
            bankContext.BankPayment.RemoveRange(_bankDbContext.BankPayment.ToList());
            bankContext.SaveChanges();
        }

        public BankPayment Pay(BankPayment bankPayment)
        {
            BankPayment bankPaymentDetails = _bankDbContext.BankPayment.SingleOrDefault(b => b.Id == bankPayment.Id);
            _logger.LogInformation("Creating stripe payment");
            StripeConfiguration.ApiKey = API_KEY;
            bankPaymentDetails.Number = bankPayment.Number;
            bankPaymentDetails.Expiration = bankPayment.Expiration;
            bankPaymentDetails.SecurityCode = bankPayment.SecurityCode;
            var chargeService = new ChargeService();
            String value = bankPaymentDetails.Amount.ToString("0.00").Replace(',', '.');
            float amount = float.Parse(value);

            var options = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = bankPaymentDetails.Number,
                    ExpMonth = bankPaymentDetails.Expiration.Month.ToString(),
                    ExpYear = bankPaymentDetails.Expiration.Year.ToString(),
                    Name = bankPaymentDetails.FirstName + " " + bankPaymentDetails.LastName,
                    Cvc = bankPaymentDetails.SecurityCode,
                    AddressLine1 = bankPaymentDetails.Address.Street,
                    AddressCity = bankPaymentDetails.Address.City,
                    AddressCountry = "Serbia",
                },
            };
            var service = new TokenService();
            Token token = service.Create(options);

            var chargeOptions = new ChargeCreateOptions
            {
                Amount = (int)Math.Round(amount * 100f),
                Currency = "usd",
                Description = "example",
                Metadata = new Dictionary<string, string>
                {
                    { "customer_id", bankPaymentDetails.MerchantId},
                    { "customer_email", bankPaymentDetails.Email },
                }
            };

            try
            {
                Charge charge = chargeService.Create(chargeOptions);
                bankPaymentDetails.PaymentApproval = PaymentApprovalType.Success;
                _bankDbContext.BankPayment.Update(bankPaymentDetails);
                return bankPaymentDetails;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                bankPaymentDetails.PaymentApproval = PaymentApprovalType.Rejected;
                _bankDbContext.BankPayment.Update(bankPaymentDetails);
                return bankPaymentDetails;
            }
        }

        public string Save(BankPayment bankPayment)
        {
            bankPayment.PaymentApproval = PaymentApprovalType.Pending;
            _bankDbContext.BankPayment.Add(bankPayment);
            return bankPayment.Id.ToString();
        }

    }
}
