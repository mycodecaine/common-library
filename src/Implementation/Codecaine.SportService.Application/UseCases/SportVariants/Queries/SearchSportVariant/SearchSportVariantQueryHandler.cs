using AutoMapper;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Pagination;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Queries.SearchSportVariant
{



    internal class SearchSportVariantQueryHandler : QueryHandler<SearchSportVariantQuery, Maybe<PagedResult<SportVariantViewModel>>>
    {

        private readonly ISportVariantRepository _sportVariantRepository;
        private readonly IMapper _mapper;

        public SearchSportVariantQueryHandler(ILogger<SearchSportVariantQueryHandler> logger, ISportVariantRepository sportTypeRepository, IMapper mapper) : base(logger)
        {
            _sportVariantRepository = sportTypeRepository;
            _mapper = mapper;
        }

        public override async Task<Maybe<PagedResult<SportVariantViewModel>>> Handle(SearchSportVariantQuery request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {            


             var (Items, TotalCount) = await _sportVariantRepository.GetPagedAsync( request.Page, request.PageSize, request.Name,request.Description,
                 request.SportTypeId, request.ImageUrl,request.IsOlympic,request.SortBy,request.IsDescending);
             var viewModels = _mapper.Map<List<SportVariantViewModel>>(Items);
             var pagedResult = viewModels.GetPaged(TotalCount, request.Page, request.PageSize);
             return pagedResult;
         });
    }
}
