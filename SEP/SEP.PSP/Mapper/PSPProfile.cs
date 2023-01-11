using AutoMapper;
using SEP.PSP.DTO;
using SEP.PSP.Models;

namespace SEP.PSP.Mapper
{
    public class PSPProfile : Profile
    {
        public PSPProfile() {
            CreateMap<PSPPayment, PSPPaymentDTO>();
            CreateMap<PSPPaymentDTO, PSPPayment>();
            CreateMap<PSPPayment, PSPBankPaymentDTO>();
            CreateMap<PSPBankPaymentDTO, PSPPayment>();
            CreateMap<PSPPayment, PSPBitcoinPaymentDTO>();
            CreateMap<PSPBitcoinPaymentDTO, PSPPayment>();
            CreateMap<Subscription, SubscriptionDTO>();
            CreateMap<SubscriptionDTO, Subscription>();
            CreateMap<Merchant, MerchantDTO>();
            CreateMap<MerchantDTO, Merchant>();
        }
    }
}
