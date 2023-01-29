using SEP.Bitcoin.DTO;
using SEP.Bitcoin.Infrastructure;
using SEP.Bitcoin.Interfaces;
using SEP.Bitcoin.Models;
using SEP.Common.Enums;
using NBitcoin;
using NBitcoin.JsonConverters;
using NBitcoin.OpenAsset;
using System.Text;
using QBitNinja.Client;
using Nancy.Json;
using System.Net;

namespace SEP.Bitcoin.Services
{
    public class BitcoinService : IBitcoinService
    {
        private readonly ILogger<BitcoinService> _logger;
        private readonly BitcoinDbContext _bitcoinDbContext;
        private string DestinationAddress { get; set; }

        public BitcoinService(ILogger<BitcoinService> logger, BitcoinDbContext bitcoinDbContext)
        {
            _logger = logger;
            _bitcoinDbContext = bitcoinDbContext;

            _bitcoinDbContext.BitcoinPayment.RemoveRange(_bitcoinDbContext.BitcoinPayment.ToList());
            _bitcoinDbContext.SaveChanges();
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            DestinationAddress = appSettings.GetValue<string>("DestinationAddress");
        }
        public BitcoinPayment GetById(string id)
        {
            BitcoinPayment bitcoinPaymentDetails = _bitcoinDbContext.BitcoinPayment.SingleOrDefault(b => b.Id == int.Parse(id));
            return bitcoinPaymentDetails;
        }
        public BitcoinPayment Pay(BitcoinPayment bitcoinPayment)
        {
            Key privateKeyy = new Key(StringToByteArray(bitcoinPayment.PrivateKey));
            var privateKey = new BitcoinSecret(privateKeyy, Network.TestNet);
            var client = new QBitNinjaClient(Network.TestNet);
            var destinationAddress = new BitcoinPubKeyAddress(DestinationAddress, Network.TestNet);
            var address = privateKey.GetAddress(ScriptPubKeyType.Legacy);
            //mw2DzXinK8KaqunpYgjnGyCYcgHVb3SJWc
            var balance = client.GetBalance(address, true).Result;
            var builder = Network.TestNet.CreateTransactionBuilder();
            var jss = new JavaScriptSerializer();
            var getdata = string.Empty;
            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://blockchain.info/tobtc?currency="+bitcoinPayment.Currency.ToUpper()+"&value="+bitcoinPayment.Amount);
            httpRequest.Method = "GET";
            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            var coin = balance.Operations
            .SelectMany(op => op.ReceivedCoins)
            .FirstOrDefault(c => c.Amount == balance.Operations.SelectMany(op => op.ReceivedCoins).Max(c => c.Amount));
            var feeAmount = new Money(0.001m, MoneyUnit.BTC);
            var transaction = builder
                .AddCoins(coin)
                .AddKeys(privateKey)
                .Send(destinationAddress, new Money(Decimal.Parse(getdata.Replace(".", ",00")), MoneyUnit.BTC))
                .SetChange(privateKey.GetAddress(ScriptPubKeyType.Legacy))
                .SendFees(feeAmount)
                .BuildTransaction(false);

            var broadcastResponse = client.Broadcast(transaction).Result;

            if (broadcastResponse.Success)
            {
                bitcoinPayment.PaymentApproval = PaymentApprovalType.Success;
                _bitcoinDbContext.BitcoinPayment.Add(bitcoinPayment);
                _bitcoinDbContext.SaveChanges();
                _logger.LogInformation("Transaction broadcast successfully!");

            }
            else
            {
                bitcoinPayment.PaymentApproval = PaymentApprovalType.Rejected;
                _bitcoinDbContext.BitcoinPayment.Add(bitcoinPayment);
                _bitcoinDbContext.SaveChanges();
                _logger.LogInformation("Transaction broadcast failed.");
            }
            return bitcoinPayment;
        }

        public string Save(BitcoinPayment bitcoinPayment)
        {
            bitcoinPayment.PaymentApproval = PaymentApprovalType.Pending;
            _bitcoinDbContext.BitcoinPayment.Add(bitcoinPayment);
            _bitcoinDbContext.SaveChanges();
            return bitcoinPayment.Id.ToString();
        }

        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

    }
}
