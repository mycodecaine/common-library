using Codecaine.Common.CQRS.Base;
using Codecaine.Common.OpenAiServices.Interfaces;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.Documents.Queries.SearchDocumentByVector
{


    internal class SearchDocumentByVectorQueryHandler : QueryHandler<SearchDocumentByVectorQuery, Maybe<List<DocumentViewModel>>>
    {
        private readonly ILogger<SearchDocumentByVectorQueryHandler> _logger;
        private readonly IDocumentRepository _repository;      
     
        public SearchDocumentByVectorQueryHandler(ILogger<SearchDocumentByVectorQueryHandler> logger, IDocumentRepository repository) : base(logger)
        {
            _logger = logger;
            _repository = repository;           
           
        }

        public override async Task<Maybe<List<DocumentViewModel>>> Handle(SearchDocumentByVectorQuery request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
           
             var documentResult = await _repository.SearchContentVectorAsync(request.content);
             if (!documentResult.Any())
             {
                 _logger.LogWarning("Content not found");
                 return Maybe<List<DocumentViewModel>>.None;
             }

             var document = documentResult.Select(x => x.Content);
             var playerViewModel = document.Select(x => new DocumentViewModel(x));


             return playerViewModel.ToList();
            
         });
    }

}
