using Codecaine.Common.Abstractions;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Persistence;
using Codecaine.Common.Primitives.Errors;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType
{
    /// <summary>
    /// Command handler for creating a sport type.
    /// </summary>
    internal sealed class CreateSportTypeCommandHandler : CommandHandler<CreateSportTypeCommand, Result<CreateSportTypeCommandResponse>>
    {
        private readonly ILogger<CreateSportTypeCommandHandler> _logger;
        private readonly ISportTypeRepository _sportTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;

        /// <summary>
        /// Constructor for CreateSportTypeCommandHandler.
        /// </summary>
        /// <param name="sportTypeRepository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="requestContext"></param>
        /// <param name="logger"></param>
        public CreateSportTypeCommandHandler(ISportTypeRepository sportTypeRepository , IUnitOfWork unitOfWork,
             IRequestContext requestContext, ILogger<CreateSportTypeCommandHandler> logger) : base(logger)
        {
            _logger = logger;
            _sportTypeRepository = sportTypeRepository;
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
        }

        /// <summary>
        /// Handles the command to create a sport type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<CreateSportTypeCommandResponse>> Handle(CreateSportTypeCommand request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             var exist = await _sportTypeRepository.IsNameExistAsync(request.Name);
             if (exist)
             {
                 _logger.LogWarning("Sport type with name: {Name} already exists", request.Name);
                 return Result.Failure<CreateSportTypeCommandResponse>( new Error("SportTypeNameExist", $"Sport type with name: {request.Name} already exists") );
             }

             var sportType = SportType.Create(request.Name,request.Description,request.ImageUrl);

             _logger.LogInformation("Creating sport type with name: {Name}", request.Name);
             _sportTypeRepository.Insert(sportType);


             await _unitOfWork.SaveChangesAsync(_requestContext.UserId, cancellationToken);
             _logger.LogInformation("Sport type with name: {Name} created", request.Name);

             return Result.Success(new CreateSportTypeCommandResponse(sportType.Id));

         });
    }
}
