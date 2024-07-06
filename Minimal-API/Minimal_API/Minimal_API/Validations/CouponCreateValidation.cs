using FluentValidation;
using Minimal_API.Models.DTO;


namespace Minimal_API.Validations
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateDTO>
    {
        public CouponCreateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1,100);
        }
    
    }
}
