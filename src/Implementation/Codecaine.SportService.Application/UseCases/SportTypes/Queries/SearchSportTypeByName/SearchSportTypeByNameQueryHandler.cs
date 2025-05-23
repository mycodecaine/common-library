﻿using AutoMapper;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Pagination;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Queries.SearchSportTypeByName
{


    internal class SearchSportTypeByNameQueryHandler : QueryHandler<SearchSportTypeByNameQuery, Maybe<PagedResult<SportTypeViewModel>>>
    {
        
        private readonly ISportTypeRepository _sportTypeRepository;
        private readonly IMapper _mapper;

        public SearchSportTypeByNameQueryHandler(ILogger<SearchSportTypeByNameQueryHandler> logger, ISportTypeRepository sportTypeRepository, IMapper mapper) : base(logger)
        {           
            _sportTypeRepository = sportTypeRepository;
            _mapper = mapper;
        }

        public override async Task<Maybe<PagedResult<SportTypeViewModel>>> Handle(SearchSportTypeByNameQuery request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             List<FilterCriterion> filterCriterion = [];

             if (!string.IsNullOrWhiteSpace(request.Name))
             {
               filterCriterion.Add(new FilterCriterion
               {
                   Property = nameof(SportType.Name),
                   Operator = FilterOperator.Contains,
                   Values = [request.Name.Trim().ToLower()]
               });
             }

             var specification = SpecificationBuilder.Build<SportType>(filterCriterion);

             var (Items, TotalCount) = await _sportTypeRepository.GetPagedAsync(request.Page, request.PageSize, specification, nameof(SportTypeViewModel.Name), false);            
             var viewModels = _mapper.Map<List<SportTypeViewModel>>(Items);
             var pagedResult = viewModels.GetPaged(TotalCount, request.Page, request.PageSize);            
             return pagedResult;
         });
    }
}
