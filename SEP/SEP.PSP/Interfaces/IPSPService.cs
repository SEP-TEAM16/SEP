using Microsoft.AspNetCore.Mvc;
using SEP.PSP.DTO;
using SEP.PSP.Models;

namespace SEP.PSP.Interfaces
{
    public interface IPSPService
    {
        public Subscription SubscribeWebshopToPayment(Subscription subscription);
        public PSPPaymentDTO MakePayPalPayment(PSPPaymentDTO PSPPaymentDTO);
    }
}
