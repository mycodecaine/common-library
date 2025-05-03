using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetSportTypeByIdQueryHandler(ILogger<GetSportTypeByIdQueryHandler> logger, ISportTypeRepository sportTypeRepository, IMapper mapper) : base(logger)
        {
            _logger = logger;
            _sportTypeRepository = sportTypeRepository;
            _mapper = mapper;
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
             var sportTypeViewModel = _mapper.Map<SportTypeViewModel>(sportType);
             return sportTypeViewModel;
         });
    }
}
