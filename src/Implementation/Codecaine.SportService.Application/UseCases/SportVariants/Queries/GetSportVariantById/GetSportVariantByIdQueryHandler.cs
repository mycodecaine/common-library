using AutoMapper;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.Mappers;
using Codecaine.SportService.Application.UseCases.SportTypes.Queries.GetSportTypeById;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Queries.GetSportVariantById
{
   
    internal class GetSportVariantByIdQueryHandler : QueryHandler<GetSportVariantByIdQuery, Maybe<SportVariantViewModel>>
    {
        private readonly ILogger<GetSportVariantByIdQueryHandler> _logger;
        private readonly ISportVariantRepository _sportVariantRepository;
        private readonly IMapper _mapper;

        public GetSportVariantByIdQueryHandler(ILogger<GetSportVariantByIdQueryHandler> logger, ISportVariantRepository sportVariantRepository, IMapper mapper) : base(logger)
        {
            _logger = logger;
            _sportVariantRepository = sportVariantRepository;
            _mapper = mapper;
        }

        public override async Task<Maybe<SportVariantViewModel>> Handle(GetSportVariantByIdQuery request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             var sportVariantResult = await _sportVariantRepository.GetByIdAsync(request.Id);
             if (sportVariantResult.HasNoValue)
             {
                 _logger.LogWarning("Sport variant with id: {Id} not found", request.Id);
                 return Maybe<SportVariantViewModel>.None;
             }

             var sportVariant = sportVariantResult.Value;
             var viewModel = _mapper.Map<SportVariantViewModel>(sportVariant);           
             return viewModel;
         });
    }
}
