using Codecaine.Common.Abstractions;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Persistence;
using Codecaine.Common.Primitives.Errors;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Codecaine.SportService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Commands.CreateSportVariant
{
    public sealed class CreateSportVariantCommandHandler : CommandHandler<CreateSportVariantCommand, Result<CreateSportVariantCommandResponse>>
    {
        private readonly ILogger<CreateSportVariantCommandHandler> _logger;
        private readonly ISportVariantRepository _sportVariantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;
        public CreateSportVariantCommandHandler(ISportVariantRepository sportVariantRepository, IUnitOfWork unitOfWork,
             IRequestContext requestContext, ILogger<CreateSportVariantCommandHandler> logger) : base(logger)
        {
            _logger = logger;
            _sportVariantRepository = sportVariantRepository;
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;

        }

        public async override Task<Result<CreateSportVariantCommandResponse>> Handle(CreateSportVariantCommand request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             var exist = await _sportVariantRepository.IsNameExistAsync( request.SportTypeId, request.Name);
             if (exist)
             {
                 _logger.LogWarning("Sport variant with name: {Name} already exists", request.Name);
                 return Result.Failure<CreateSportVariantCommandResponse>(new Error("SportVariantNameExist", $"Sport variant with name: {request.Name} already exists"));
             }

             Result<SportRule> sportRule = SportRule.Create(request.RuleScoringSystem, request.RulePlayerCount, request.RuleGameDuration, request.RuleMaxScore);

             Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(sportRule);

             if (firstFailureOrSuccess.IsFailure)
             {
                 _logger.LogWarning("Sport Rule with variant name: {Name} not valid", request.Name);

                 return Result.Failure<CreateSportVariantCommandResponse>(firstFailureOrSuccess.Error);
             }

             var sportVariant = SportVariant.Create(request.Name, request.Description, request.ImageUrl, request.IsOlympic, request.SportTypeId, sportRule.Value);

             if (request.PopularInCountries != null && request.PopularInCountries.Count != 0)
             {
                 foreach (var popularInCountries in request.PopularInCountries)
                 {
                     sportVariant.AddPopularInCountry(popularInCountries.CountryCode, popularInCountries.Popularity);
                 }
             }

             if (request.PlayerPositions != null && request.PlayerPositions.Count != 0)
             {
                 foreach (var playerPosition in request.PlayerPositions)
                 {
                     sportVariant.AddPlayerPosition(playerPosition.Name, playerPosition.Description,playerPosition.ImageUrl,playerPosition.Responsibilities);
                 }
             }

             _logger.LogInformation("Creating sport variant with name: {Name}", request.Name);
             _sportVariantRepository.Insert(sportVariant);


             await _unitOfWork.SaveChangesAsync(_requestContext.UserId, cancellationToken);
             _logger.LogInformation("Sport variant with name: {Name} created", request.Name);

             return Result.Success(new CreateSportVariantCommandResponse(sportVariant.Id));

         });
    }
}
