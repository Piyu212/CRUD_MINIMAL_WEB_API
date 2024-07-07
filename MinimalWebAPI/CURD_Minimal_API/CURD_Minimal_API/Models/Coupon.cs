namespace CRUD_Minimal_API.Models
{
    public class Coupon
    {
        public int Id { get; set; }   //coupon id
        public string Name { get; set; } // Coupon name
        public int Percent { get; set; }  //what percentage discout wil coupon give
        public bool IsAactive { get; set; } //coupon is active or not
        public DateTime? Created { get; set; } 
        public DateTime? LastUpdated { get; set; }
    }
}
