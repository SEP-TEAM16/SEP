using SEP.Bank.DTO;
using SEP.Bank.Models;

namespace SEP.Bank.Interfaces
{
    public interface IBankService
    {
        public BankPayment Pay(CardDTO cardDTO);
        public string Save(BankPayment bankPayment);
    }
}
