using Codecaine.Common.CQRS.Queries;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.Players.Queries.GetPlayerById
{

    public record GetPlayerByIdQuery(Guid Id) : IQuery<Maybe<PlayerViewModel>>;
}

