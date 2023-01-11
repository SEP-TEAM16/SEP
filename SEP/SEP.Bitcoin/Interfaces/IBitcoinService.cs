using SEP.Bitcoin.DTO;
using SEP.Bitcoin.Models;

namespace SEP.Bitcoin.Interfaces
{
    public interface IBitcoinService
    {
        public BitcoinPayment Pay(BitcoinPayment bitcoinPayment);
        public BitcoinPayment GetById(string id);
        public string Save(BitcoinPayment bitcoinPayment);
    }
}
