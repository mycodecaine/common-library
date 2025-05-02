using Codecaine.Common.Extensions;
using Codecaine.Common.Primitives.Errors;
using FluentValidation;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.UpdateSportType
{
    internal class UpdateSportTypeCommandValidator : AbstractValidator<UpdateSportTypeCommand>
    {
        public UpdateSportTypeCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithError(new Error("DescriptionNullOrEmpty", "Description Null or Empty"));

            RuleFor(x => x.Name).NotEmpty().WithError(new Error("NameNullOrEmpty", "Name Null or Empty"));
        }
    }
}
