//We never want to show when coupon was last updated.
namespace Minimal_API.Models.DTO
{
    public class CouponDTO
    {
        public int Id { get; set; }   //coupon id
        public string Name { get; set; } //coupon name
        public int Percent { get; set; } // What percentage discount will that coupon give
        public bool IsActive { get; set; } //if that coupon is active or not
        public DateTime? Created { get; set; }
    }
}
