using FluentValidation;
using Vml.Core.Dtos;

namespace Vml.Application.Validators
{
    public class DataJobDtoValidator : AbstractValidator<DataJobDTO>
    {
        public DataJobDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.FilePathToProcess)
                .NotEmpty()
                .MaximumLength(500);

            RuleForEach(x => x.Links).ChildRules(x =>
            {
                x.RuleFor(x => x.Rel).NotEmpty().MaximumLength(200);
                x.RuleFor(x => x.Href).NotEmpty().MaximumLength(200);
                x.RuleFor(x => x.Action).NotEmpty().MaximumLength(200);
            });
        }
    }
}
