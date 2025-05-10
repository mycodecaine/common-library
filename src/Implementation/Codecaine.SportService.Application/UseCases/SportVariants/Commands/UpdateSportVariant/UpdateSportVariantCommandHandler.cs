using Codecaine.Common.Abstractions;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Exceptions;
using Codecaine.Common.Persistence;
using Codecaine.Common.Primitives.Errors;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Codecaine.SportService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Commands.UpdateSportVariant
{


    internal sealed class UpdateSportVariantCommandHandler : CommandHandler<UpdateSportVariantCommand, Result<UpdateSportVariantCommandResponse>>
    {
        private readonly ILogger<UpdateSportVariantCommandHandler> _logger;
        private readonly ISportVariantRepository _sportVariantRepository;
        private readonly ISportTypeRepository _sportTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;
        public UpdateSportVariantCommandHandler(ISportVariantRepository sportVariantRepository, ISportTypeRepository sportTypeRepository, IUnitOfWork unitOfWork,
             IRequestContext requestContext, ILogger<UpdateSportVariantCommandHandler> logger) : base(logger)
        {
            _logger = logger;
            _sportVariantRepository = sportVariantRepository;
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
            _sportTypeRepository = sportTypeRepository;

        }

        public async override Task<Result<UpdateSportVariantCommandResponse>> Handle(UpdateSportVariantCommand request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             // Check if sport type exists
             var sportTypeResult = await _sportTypeRepository.GetByIdAsync(request.SportTypeId);
             if (sportTypeResult.HasNoValue)
             {
                 _logger.LogWarning("Sport type with id: {Id} not found", request.SportTypeId);
                 throw new NotFoundException(new Error("SportTypeNotFound", $"Sport type with id: {request.SportTypeId} not found"));
             }

             var sportVariantResult = await _sportVariantRepository.GetByIdAsync(request.Id);
             if (sportVariantResult.HasNoValue)
             {
                 _logger.LogWarning("Sport variant with id: {Id} not found", request.Id);
                 throw new NotFoundException(new Error("SportVariant", $"Sport variant with id: {request.Id} not found"));
             }

             // Check if sport variant name already exists
             var exist = await _sportVariantRepository.IsDuplicateNameAsync(request.Id, request.SportTypeId, request.Name);
             if (exist)
             {
                 _logger.LogWarning("Sport variant with name: {Name} already exists", request.Name);
                 return Result.Failure<UpdateSportVariantCommandResponse>(new Error("SportVariantNameExist", $"Sport variant with name: {request.Name} already exists"));
             }

             // Create sport rule value object
             Result<SportRule> sportRule = SportRule.Create(request.RuleScoringSystem, request.RulePlayerCount, request.RuleGameDuration, request.RuleMaxScore);

             Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(sportRule);

             if (firstFailureOrSuccess.IsFailure)
             {
                 _logger.LogWarning("Sport Rule with variant name: {Name} not valid", request.Name);

                 return Result.Failure<UpdateSportVariantCommandResponse>(firstFailureOrSuccess.Error);
             }

             var sportVariant = sportVariantResult.Value;

             sportVariant.Update(request.Name, request.Description, request.ImageUrl, request.IsOlympic, request.SportTypeId, sportRule.Value);     

             // Update sport variant properties
             if (request.PopularInCountries != null && request.PopularInCountries.Count != 0)
             {
                 foreach (var popularInCountries in request.PopularInCountries)
                 {
                     sportVariant.UpdatePopularInCountry(popularInCountries.Id, popularInCountries.CountryCode, popularInCountries.Popularity);
                 }
             }             

             // Update player positions properties
             if (request.PlayerPositions != null && request.PlayerPositions.Count != 0)
             {
                 foreach (var playerPosition in request.PlayerPositions)
                 {
                     sportVariant.UpdatePlayerPosition(playerPosition.Id, playerPosition.Name, playerPosition.Description, playerPosition.ImageUrl, playerPosition.Responsibilities);
                 }
             }
             _logger.LogInformation("Creating sport variant with name: {Name}", request.Name);


             _sportVariantRepository.Update(sportVariant);
             await _unitOfWork.SaveChangesAsync(_requestContext.UserId, cancellationToken);
             _logger.LogInformation("Sport variant with name: {Name} created", request.Name);

             return Result.Success(new UpdateSportVariantCommandResponse());

         });
    }

}
