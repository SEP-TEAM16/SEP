using AutoMapper;
using SEP.Bank2.DTO;
using SEP.Bank2.Models;

namespace SEP.Bank2.Mapper
{
    public class BankProfile : Profile
    {
        public BankProfile()
        {
            CreateMap<BankPayment, BankPaymentDTO>();
            CreateMap<BankPaymentDTO, BankPayment>();
            CreateMap<BankPayment, BankPaymentWithoutCardDTO>();
            CreateMap<BankPaymentWithoutCardDTO, BankPayment>();
        }
    }
}
