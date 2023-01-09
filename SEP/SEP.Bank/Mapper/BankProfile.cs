using AutoMapper;
using SEP.Bank.DTO;
using SEP.Bank.Models;

namespace SEP.Bank.Mapper
{
    public class BankProfile : Profile
    {
        public BankProfile()
        {
            CreateMap<BankPayment, BankPaymentDTO>();
            CreateMap<BankPaymentDTO, BankPayment>();
        }
    }
}
