using SEP.PSP.DTO;
using SEP.PSP.Models;

namespace SEP.PSP.Interfaces
{
    public interface IPSPService
    {
        public Subscription SubscribeWebshopToPayment(Subscription subscription);
        public string MakePayPalPayment(PSPPaymentDTO PSPPaymentDTO);
        public string MakeBankPayment(PSPPaymentDTO PSPPaymentDTO);
        public void MakeBitcoinPayment(PSPBitcoinPaymentDTOForReceive PSPBitcoinPaymentDTO);
        public string MakeQRPayment(PSPPaymentDTO PSPPaymentDTO);
        public void EditPayPalPayment(PSPPayment PSPPayment);
        public void UpdatePayment(PSPPayment PSPPayment);
        public List<Subscription> GetSubscribedByPort(string port);
        public Boolean RemoveServiceType(string serviceType, string port);
        public Merchant GetMerchantByPort(string port);
    }
}
