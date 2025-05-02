using Codecaine.Common.CQRS.Base;
using Codecaine.Common.CQRS.Queries;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Queries.GetSportTypeById
{
    internal class GetSportTypeByIdQueryHandler : QueryHandler<GetSportTypeByIdQuery, Maybe<SportTypeViewModel>>
    {
        private readonly ILogger<GetSportTypeByIdQueryHandler> _logger;
        private readonly ISportTypeRepository _sportTypeRepository;

        public GetSportTypeByIdQueryHandler(ILogger<GetSportTypeByIdQueryHandler> logger, ISportTypeRepository sportTypeRepository) : base(logger)
        {
            _logger = logger;
            _sportTypeRepository = sportTypeRepository;
        }

        public override async Task<Maybe<SportTypeViewModel>> Handle(GetSportTypeByIdQuery request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             var sportTypeResult = await _sportTypeRepository.GetByIdAsync(request.Id);
             if (sportTypeResult.HasNoValue)
             {
                 _logger.LogWarning("Sport type with id: {Id} not found", request.Id);
                 return Maybe<SportTypeViewModel>.None;
             }

             var sportType = sportTypeResult.Value;
             var sportTypeViewModel = new SportTypeViewModel
             (
                  sportType.Id,
                  sportType.Name,
                  sportType.Description,
                  sportType.ImageUrl

             );

             return sportTypeViewModel;
         });
    }
}
