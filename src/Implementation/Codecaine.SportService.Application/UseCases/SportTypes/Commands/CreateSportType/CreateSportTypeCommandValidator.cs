using Codecaine.Common.Extensions;
using Codecaine.Common.Primitives.Errors;
using FluentValidation;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType
{
    /// <summary>
    /// Validator for the CreateSportTypeCommand.
    /// Ensures that the Name and Description properties are not null or empty.
    /// </summary>
    public sealed class CreateSportTypeCommandValidator : AbstractValidator<CreateSportTypeCommand>
    {
        public CreateSportTypeCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithError(new Error("DescriptionNullOrEmpty", "Description Null or Empty"));

            RuleFor(x => x.Name).NotEmpty().WithError(new Error("NameNullOrEmpty", "Name Null or Empty"));
        }
    }
}
