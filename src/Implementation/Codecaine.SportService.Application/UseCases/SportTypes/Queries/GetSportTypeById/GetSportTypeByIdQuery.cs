using Codecaine.Common.CQRS.Queries;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Queries.GetSportTypeById
{

    /// <summary>
    /// Query to get a sport type by its ID.
    /// </summary>
    /// <param name="Id"></param>
    public record GetSportTypeByIdQuery(Guid Id) : IQuery<Maybe<SportTypeViewModel>>;
}
