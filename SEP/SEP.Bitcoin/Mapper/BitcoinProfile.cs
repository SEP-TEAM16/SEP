using AutoMapper;
using SEP.Bitcoin.DTO;
using SEP.Bitcoin.Models;

namespace SEP.Bitcoin.Mapper
{
    public class BitcoinProfile : Profile
    {
        public BitcoinProfile()
        {
            CreateMap<BitcoinPayment, BitcoinPaymentDTO>();
            CreateMap<BitcoinPaymentDTO, BitcoinPayment>();
            CreateMap<BitcoinPayment, BitcoinPaymentWithoutKeysDTO>();
            CreateMap<BitcoinPaymentWithoutKeysDTO, BitcoinPayment>();
            CreateMap<BitcoinPaymentWithoutKeysDTO, BitcoinPaymentDTO>();
            CreateMap<BitcoinPaymentDTO, BitcoinPaymentWithoutKeysDTO>();
        }
    }
}
