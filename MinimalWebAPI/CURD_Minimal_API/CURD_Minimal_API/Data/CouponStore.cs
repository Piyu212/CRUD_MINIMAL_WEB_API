using CRUD_Minimal_API.Models;

namespace CRUD_Minimal_API.Data
{
    public class CouponStore
    {
        public static List<Coupon> couponList = new List<Coupon>  //static class ghetal karan every time object create kryla lagnar nahi
        {
            new Coupon{Id = 1, Name = "10 OFF" , Percent=10, IsAactive=true },
            new Coupon{Id = 2, Name = "20 OFF" , Percent=20, IsAactive=false },
        };
    }
}
