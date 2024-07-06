//DTO (Data Transfer Object) is a design pattern used to transfer data between software application subsystems.
namespace Minimal_API.Models.DTO
{
    public class CouponCreateDTO
    {
        public string Name { get; set; } //coupon name
        public int Percent { get; set; } // What percentage discount will that coupon give
        public bool IsActive { get; set; } //if that coupon is active or not
    }
}
