using Codecaine.Common.Abstractions;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Persistence;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType
{
    internal sealed class CreateSportTypeCommandHandler : CommandHandler<CreateSportTypeCommand, Result<CreateSportTypeCommandResponse>>
    {
        private readonly ILogger<CreateSportTypeCommandHandler> _logger;
        private readonly ISportTypeRepository _sportTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;


        public CreateSportTypeCommandHandler(ISportTypeRepository sportTypeRepository , IUnitOfWork unitOfWork,
             IRequestContext requestContext, ILogger<CreateSportTypeCommandHandler> logger) : base(logger)
        {
            _logger = logger;
            _sportTypeRepository = sportTypeRepository;
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
        }

        public override async Task<Result<CreateSportTypeCommandResponse>> Handle(CreateSportTypeCommand request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             var sportType = SportType.Create(request.Name,request.Description,request.ImageUrl);

             _logger.LogInformation("Creating sport type with name: {Name}", request.Name);
             _sportTypeRepository.Insert(sportType);


             await _unitOfWork.SaveChangesAsync(_requestContext.UserId, cancellationToken);
             _logger.LogInformation("Sport type with name: {Name} created", request.Name);

             return Result.Success(new CreateSportTypeCommandResponse(sportType.Id));


         });
    }
}
