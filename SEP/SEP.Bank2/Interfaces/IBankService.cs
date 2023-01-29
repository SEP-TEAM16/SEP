using SEP.Bank2.DTO;
using SEP.Bank2.Models;

namespace SEP.Bank2.Interfaces
{
    public interface IBankService
    {
        public BankPayment Pay(BankPayment bankPayment);
        public BankPayment Convert(CardDTO cardDTO);
        public BankPayment GetById(string id);
        public string Save(BankPayment bankPayment);
    }
}
