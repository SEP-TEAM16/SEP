using SEP.Bank2.DTO;
using SEP.Bank2.Infrastructure;
using SEP.Bank2.Interfaces;
using SEP.Bank2.Models;
using SEP.Common.Enums;
using Stripe;

namespace SEP.Bank2.Services
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
        public BankPayment GetById(string id)
        {
            BankPayment bankPaymentDetails = _bankDbContext.BankPayment.SingleOrDefault(b => b.Id == int.Parse(id));
            return bankPaymentDetails;
        }
        public BankPayment Pay(BankPayment bankPayment)
        {
            bankPayment.Date = DateTime.Now;
            _logger.LogInformation("Creating stripe payment");
            StripeConfiguration.ApiKey = API_KEY;
            var chargeService = new ChargeService();
            String value = bankPayment.Amount.ToString("0.00").Replace(',', '.');
            float amount = float.Parse(value);

            var options = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = bankPayment.Number,
                    ExpMonth = bankPayment.Expiration.Month.ToString(),
                    ExpYear = bankPayment.Expiration.Year.ToString(),
                    Name = bankPayment.FirstName + " " + bankPayment.LastName,
                    Cvc = bankPayment.SecurityCode,
                    AddressLine1 = "Puskinova",
                    AddressCity = "Novi Sad",
                    AddressCountry = "Serbia",
                },
            };
            var service = new TokenService();
            Token token = service.Create(options);

            var chargeOptions = new ChargeCreateOptions
            {
                Amount = (int)amount,
                Currency = "usd",
                Description = "example",
                Metadata = new Dictionary<string, string>
                {
                    { "customer_id", bankPayment.MerchantId},
                    { "customer_email", bankPayment.Email },
                },
                Source = token.Id
            };

            try
            {
                Charge charge = chargeService.Create(chargeOptions);
                bankPayment.PaymentApproval = PaymentApprovalType.Success;
                bankPayment.Id = 0;
                _bankDbContext.BankPayment.Add(bankPayment);
                _bankDbContext.SaveChanges();
                return bankPayment;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                bankPayment.PaymentApproval = PaymentApprovalType.Rejected;
                bankPayment.Id = 0;
                _bankDbContext.BankPayment.Add(bankPayment);
                _bankDbContext.SaveChanges();
                return bankPayment;
            }
        }

        public BankPayment Convert(CardDTO cardDTO)
        {
            BankPayment bankPaymentDetails = _bankDbContext.BankPayment.SingleOrDefault(b => b.Id == int.Parse(cardDTO.Id));
            bankPaymentDetails.Number = cardDTO.Number;
            bankPaymentDetails.Expiration = new DateTime();
            bankPaymentDetails.Expiration = bankPaymentDetails.Expiration.AddYears(int.Parse(cardDTO.Year) - bankPaymentDetails.Expiration.Year);
            bankPaymentDetails.Expiration = bankPaymentDetails.Expiration.AddMonths((int.Parse(cardDTO.Month)) - bankPaymentDetails.Expiration.Month);
            bankPaymentDetails.SecurityCode = cardDTO.SecurityCode;
            bankPaymentDetails.PaymentApproval = PaymentApprovalType.Pending;
            _bankDbContext.BankPayment.Update(bankPaymentDetails);
            _bankDbContext.SaveChanges();
            return bankPaymentDetails;
        }

        public string Save(BankPayment bankPayment)
        {
            bankPayment.PaymentApproval = PaymentApprovalType.Pending;
            bankPayment.Number = "";
            bankPayment.Expiration = new DateTime();
            bankPayment.SecurityCode = "";
            _bankDbContext.BankPayment.Add(bankPayment);
            _bankDbContext.SaveChanges();
            return bankPayment.Id.ToString();
        }

    }
}
