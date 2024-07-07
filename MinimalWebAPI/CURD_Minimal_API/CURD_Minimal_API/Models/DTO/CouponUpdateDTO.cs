namespace CRUD_Minimal_API.Models.DTO
{
    public class CouponUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } // Coupon name
        public int Percent { get; set; }  //what percentage discout wil coupon give
        public bool IsAactive { get; set; } //coupon is active or not
    }
}
