using Codecaine.Common.Extensions;
using Codecaine.Common.Primitives.Errors;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.CreateSportVariant;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Commands.RemovePopularInCountry
{
  

    public sealed class RemovePopularInCountryCommandValidator : AbstractValidator<RemovePopularInCountryCommand>
    {
        public RemovePopularInCountryCommandValidator()
        {           

            RuleFor(x => x.Id).NotEmpty().WithError(new Error("SportVariantIdIsNull", "Sport Variant Null or Empty"));

            RuleFor(x => x.PopularInCountryIds.Count).GreaterThan(0).WithError(new Error("PopularInCountryIdsIsEmpty", "Popular in country is empty list"));
        }
    }
}
