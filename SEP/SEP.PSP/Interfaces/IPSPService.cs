using SEP.PSP.DTO;
using SEP.PSP.Models;

namespace SEP.PSP.Interfaces
{
    public interface IPSPService
    {
        public Subscription SubscribeWebshopToPayment(Subscription subscription);
        public string MakePayPalPayment(PSPPaymentDTO PSPPaymentDTO);
        public void EditPayPalPayment(PSPPayment PSPPayment);
    }
}
