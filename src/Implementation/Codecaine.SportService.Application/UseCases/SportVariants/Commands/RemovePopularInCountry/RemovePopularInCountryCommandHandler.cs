using Codecaine.Common.Abstractions;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Exceptions;
using Codecaine.Common.Persistence;
using Codecaine.Common.Primitives.Errors;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.UpdateSportVariant;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Commands.RemovePopularInCountry
{
    internal sealed class RemovePopularInCountryCommandHandler : CommandHandler<RemovePopularInCountryCommand, Result<RemovePopularInCountryCommandResponse>>
    {
        private readonly ILogger<RemovePopularInCountryCommandHandler> _logger;
        private readonly ISportVariantRepository _sportVariantRepository;        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;

        public RemovePopularInCountryCommandHandler(ILogger<RemovePopularInCountryCommandHandler> logger, ISportVariantRepository sportVariantRepository, IUnitOfWork unitOfWork, IRequestContext requestContext):base(logger)
        {
            _logger = logger;
            _sportVariantRepository = sportVariantRepository;
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
        }

        public async override  Task<Result<RemovePopularInCountryCommandResponse>> Handle(RemovePopularInCountryCommand request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             var sportVariantResult = await _sportVariantRepository.GetByIdAsync(request.Id);
             if (sportVariantResult.HasNoValue)
             {
                 _logger.LogWarning("Sport variant with id: {Id} not found", request.Id);
                 throw new NotFoundException(new Error("SportVariant", $"Sport variant with id: {request.Id} not found"));
             }

             var sportVariant = sportVariantResult.Value;

             foreach (var id in request.PopularInCountryIds)
             {
                 sportVariant.RemovePopularInCountry(id);
                 
             }
             _sportVariantRepository.Update(sportVariant);
             await _unitOfWork.SaveChangesAsync(_requestContext.UserId, cancellationToken);

             return Result.Success(new RemovePopularInCountryCommandResponse());

         });
    }
}
