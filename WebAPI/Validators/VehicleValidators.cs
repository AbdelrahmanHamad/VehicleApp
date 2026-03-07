using FluentValidation;
using WebAPI.Models;

namespace WebAPI.Validators
{
    public class GetModelsRequestValidator : AbstractValidator<GetModelsRequest>
    {
        public GetModelsRequestValidator()
        {
            RuleFor(x => x.MakeId).GreaterThan(0);
            RuleFor(x => x.Year).InclusiveBetween(1990, DateTime.Now.Year + 1);
        }
    }

    public class GetTypesRequestValidator : AbstractValidator<GetTypesRequest>
    {
        public GetTypesRequestValidator()
        {
            RuleFor(x => x.MakeId).GreaterThan(0);
        }
    }
}
