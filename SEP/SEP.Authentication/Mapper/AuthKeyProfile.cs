using AutoMapper;
using SEP.Common.DTO;
using SEP.Common.Models;

namespace SEP.Autorization.Mapper
{
    public class AuthKeyProfile : Profile
    {
        public AuthKeyProfile()
        {
            CreateMap<AuthKeyWithKeyDTO, AuthKey>();
            CreateMap<AuthKey, AuthKeyWithKeyDTO>();
            CreateMap<AuthKeyDTO, AuthKey>();
            CreateMap<AuthKey, AuthKeyDTO>();
        }
    }
}
