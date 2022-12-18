using AutoMapper;
using SEP.PayPal.DTO;
using SEP.PayPal.Models;

namespace SEP.PayPal.Mapper
{
    public class PayPalProfile : Profile
    {
        public PayPalProfile()
        {
            CreateMap<PayPalPayment, PayPalPaymentDTO>();
            CreateMap<PayPalPaymentDTO, PayPalPayment>();
        }
    }
}
