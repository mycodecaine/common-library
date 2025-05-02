using Codecaine.Common.Abstractions;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Exceptions;
using Codecaine.Common.Persistence;
using Codecaine.Common.Primitives.Errors;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.UpdateSportType;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.DeleteSportType
{
    

    internal class DeleteSportTypeCommandHandler : CommandHandler<DeleteSportTypeCommand, Result>
    {
        private readonly ILogger<DeleteSportTypeCommandHandler> _logger;
        private readonly ISportTypeRepository _sportTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;

        /// <summary>
        /// Constructor for DeleteSportTypeCommandHandler.
        /// </summary>
        /// <param name="sportTypeRepository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="requestContext"></param>
        /// <param name="logger"></param>
        public DeleteSportTypeCommandHandler(ISportTypeRepository sportTypeRepository, IUnitOfWork unitOfWork,
             IRequestContext requestContext, ILogger<DeleteSportTypeCommandHandler> logger) : base(logger)
        {
            _logger = logger;
            _sportTypeRepository = sportTypeRepository;
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
        }
        public async override Task<Result> Handle(DeleteSportTypeCommand request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {

             var sportTypeResult = await _sportTypeRepository.GetByIdAsync(request.Id);
             if (sportTypeResult.HasNoValue)
             {
                 _logger.LogWarning("Sport type with id: {Id} not found", request.Id);
                 throw new NotFoundException(new Error("SportTypeNotFound", $"Sport type with id: {request.Id} not found"));
             }             

             var sportType = sportTypeResult.Value;             

             _logger.LogInformation("Deleting sport type with name: {Name}", sportType.Name);
             _sportTypeRepository.Delete(sportType.Id);


             await _unitOfWork.SaveChangesAsync(_requestContext.UserId, cancellationToken);
             _logger.LogInformation("Deleting sport type with name: {Name}", sportType.Name);

             return Result.Success();

         });
    }
}
