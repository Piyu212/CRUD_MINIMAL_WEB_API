
using Minimal_API.Models;
namespace Minimal_API.Data
{
    public class CouponStore
    {
        public static List<Coupon> couponList = new List<Coupon> {
        new Coupon{Id=1,Name="xyz",Percent=10,IsActive=true},
        new Coupon{Id=2,Name="abc",Percent=20,IsActive=false}

        };
    }
}
