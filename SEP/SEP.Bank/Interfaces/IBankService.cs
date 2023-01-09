using SEP.Bank.Models;

namespace SEP.Bank.Interfaces
{
    public interface IBankService
    {
        public BankPayment Pay(BankPayment bankPayment);
        public string Save(BankPayment bankPayment);
    }
}
