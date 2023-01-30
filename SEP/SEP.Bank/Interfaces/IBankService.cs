using SEP.Bank.DTO;
using SEP.Bank.Models;

namespace SEP.Bank.Interfaces
{
    public interface IBankService
    {
        public BankPayment Pay(CardDTO cardDTO);
        public BankPayment Convert(CardDTO cardDTO);
        public BankPayment GetById(string id);
        public string Save(BankPayment bankPayment);
        public BankPayment Update(BankPayment bankPayment);
    }
}
