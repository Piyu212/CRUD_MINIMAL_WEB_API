using AutoMapper;
using CRUD_Minimal_API.Models.DTO;
using CRUD_Minimal_API.Models;

namespace CRUD_Minimal_API
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
//AutoMapper: This is a library that helps in automatically mapping properties from one object to another.