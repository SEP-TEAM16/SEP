﻿using SEP.PayPal.Models;

namespace SEP.PayPal.Interfaces
{
    public interface IPayPalService
    {
        public string GetApprovalLink(PayPalPayment payPalPayment);
        public PayPalPayment Pay(string paymentId, string payerId, string token);
        public PayPalPayment Pay2(string subscription_id, string ba_token, string token);
        public PayPalPayment Cancel(string token);
        public string Subscribe(PayPalPayment payPalPayment);
    }
}
