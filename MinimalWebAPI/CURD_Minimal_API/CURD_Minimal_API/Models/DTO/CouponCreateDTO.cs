using System.ComponentModel.DataAnnotations;

namespace CRUD_Minimal_API.Models.DTO
{
    public class CouponCreateDTO
    {
       // [Required]
        public string Name { get; set; } // Coupon name
        public int Percent { get; set; }  //what percentage discout wil coupon give
        public bool IsAactive { get; set; } //coupon is active or not
    }
}
