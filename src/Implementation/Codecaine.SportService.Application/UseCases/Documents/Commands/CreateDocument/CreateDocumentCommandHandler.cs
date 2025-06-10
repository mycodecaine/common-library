using Codecaine.Common.CQRS.Base;
using Codecaine.Common.OpenAiServices.Interfaces;
using Codecaine.Common.Persistence.Dapper.Interfaces;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.Documents.Commands.CreateDocument
{
    internal sealed class CreateDocumentCommandHandler : CommandHandler<CreateDocumentCommand, Result<CreateDocumentCommandResponse>>
    {
        private readonly ILogger<CreateDocumentCommandHandler> _logger;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDapperUnitOfWork _unitOfWork;
        private readonly IOpenAiEmbeddingService _openAiEmbeddingService;
        public CreateDocumentCommandHandler(ILogger<CreateDocumentCommandHandler> logger, IDocumentRepository documentRepository, IDapperUnitOfWork unitOfWork, IOpenAiEmbeddingService openAiEmbeddingService) : base(logger)
        {
            _logger = logger;
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
            _openAiEmbeddingService = openAiEmbeddingService;
        }

        public override Task<Result<CreateDocumentCommandResponse>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        =>  HandleSafelyAsync(async () =>
        {
            _logger.LogInformation("CreateDocumentCommandHandler: {Content}", request.Content);
            var vector = await _openAiEmbeddingService.GetEmbeddingAsync(request.Content);
            // Create a new document entity
            var document = Document.Create(request.Content,"","");
            await _unitOfWork.StartTransactionAsync(Guid.NewGuid());
            // Save the document to the repository
            await _documentRepository.Insert(document);

            // Commit the transaction
            await _unitOfWork.CommitAsync(document);
            // Return the response with the created document ID
            return Result<CreateDocumentCommandResponse>.Success(new CreateDocumentCommandResponse(document.Id));
        });
        
       
    }
}
