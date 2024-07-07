using FluentValidation;
using CRUD_Minimal_API.Models.DTO;
using FluentValidation.Internal;

namespace CRUD_Minimal_API.Validations
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
