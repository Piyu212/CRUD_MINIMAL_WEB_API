using FluentValidation;
using Minimal_API.Models.DTO;

namespace Minimal_API.Validation
{

    public class CouponUpdatValidation : AbstractValidator<CouponUpdateDTO>
    {

        public CouponUpdatValidation()
        {
            RuleFor(model => model.Id).NotEmpty().GreaterThan(0);
            //  RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);

        }
    }
}
