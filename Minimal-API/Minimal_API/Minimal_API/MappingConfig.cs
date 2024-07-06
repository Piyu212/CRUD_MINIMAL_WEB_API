using AutoMapper;
using Minimal_API.Models;
using Minimal_API.Models.DTO;

namespace Minimal_API
{
    public class MappingConfig : Profile
    {

        public MappingConfig() 
        
        {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();


        }
    }
}
