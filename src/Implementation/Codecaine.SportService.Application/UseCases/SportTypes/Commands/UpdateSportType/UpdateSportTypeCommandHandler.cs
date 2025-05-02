using Codecaine.Common.Abstractions;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Persistence;
using Codecaine.Common.Primitives.Errors;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.UpdateSportType
{
    internal class UpdateSportTypeCommandHandler : CommandHandler<UpdateSportTypeCommand, Result<UpdateSportTypeCommandResponse>>
    {
        private readonly ILogger<UpdateSportTypeCommandHandler> _logger;
        private readonly ISportTypeRepository _sportTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;

        /// <summary>
        /// Constructor for UpdateSportTypeCommandHandler.
        /// </summary>
        /// <param name="sportTypeRepository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="requestContext"></param>
        /// <param name="logger"></param>
        public UpdateSportTypeCommandHandler(ISportTypeRepository sportTypeRepository, IUnitOfWork unitOfWork,
             IRequestContext requestContext, ILogger<UpdateSportTypeCommandHandler> logger) : base(logger)
        {
            _logger = logger;
            _sportTypeRepository = sportTypeRepository;
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
        }
        public async override Task<Result<UpdateSportTypeCommandResponse>> Handle(UpdateSportTypeCommand request, CancellationToken cancellationToken)=>
         await HandleSafelyAsync(async () =>
         {
             var exist = await _sportTypeRepository.IsDuplicateNameAsync(request.Id,request.Name);
             if (exist)
             {
                 _logger.LogWarning("Sport type with name: {Name} already exists", request.Name);
                 return Result.Failure<UpdateSportTypeCommandResponse>(new Error("SportTypeNameExist", $"Sport type with name: {request.Name} already exists"));
             }

             var sportTypeResult = await _sportTypeRepository.GetByIdAsync(request.Id);
             if (sportTypeResult.HasNoValue)
             {
                 _logger.LogWarning("Sport type with id: {Id} not found", request.Id);
                 return Result.Failure<UpdateSportTypeCommandResponse>(new Error("SportTypeNotFound", $"Sport type with id: {request.Id} not found"));
             }

             var sportType = sportTypeResult.Value;

             sportType.Update(request.Name, request.Description, request.ImageUrl);

             _logger.LogInformation("Updating sport type with name: {Name}", request.Name);
             _sportTypeRepository.Update(sportType);


             await _unitOfWork.SaveChangesAsync(_requestContext.UserId, cancellationToken);
             _logger.LogInformation("Sport type with name: {Name} created", request.Name);

             return Result.Success(new UpdateSportTypeCommandResponse());

         });
    }

}
