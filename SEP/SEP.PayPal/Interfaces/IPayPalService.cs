using SEP.PayPal.Models;

namespace SEP.PayPal.Interfaces
{
    public interface IPayPalService
    {
        public string GetApprovalLink(PayPalPayment payPalPayment);
        public bool Pay(string paymentId, string payerId, string token);
        public void Cancel(string token);
    }
}
