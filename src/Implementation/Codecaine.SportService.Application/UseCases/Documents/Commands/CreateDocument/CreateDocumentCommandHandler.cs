using Codecaine.Common.AiServices.Interfaces;
using Codecaine.Common.CQRS.Base;
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
        
        public CreateDocumentCommandHandler(ILogger<CreateDocumentCommandHandler> logger, IDocumentRepository documentRepository, IDapperUnitOfWork unitOfWork) : base(logger)
        {
            _logger = logger;
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
            
        }

        public override Task<Result<CreateDocumentCommandResponse>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        =>  HandleSafelyAsync(async () =>
        {
            _logger.LogInformation("CreateDocumentCommandHandler: {Content}", request.Content);
          
            // Create a new document entity
            var document = Document.Create(request.Name,request.Description,request.Content);
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
