using Codecaine.Common.Extensions;
using Codecaine.Common.Primitives.Errors;
using FluentValidation;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Commands.CreateSportVariant
{


    public sealed class CreateSportVariantCommandValidator : AbstractValidator<CreateSportVariantCommand>
    {
        public CreateSportVariantCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithError(new Error("DescriptionNullOrEmpty", "Description Null or Empty"));

            RuleFor(x => x.Name).NotEmpty().WithError(new Error("NameNullOrEmpty", "Name Null or Empty"));

            RuleFor(x => x.SportTypeId).NotEmpty().WithError(new Error("SportTypeId", "SportTypeId Null or Empty"));
        }
    }
}
