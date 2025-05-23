# Creating Command UseCase

This guide explains how to set up new Command use cases.

---

1. Under Application Layer Project. Create UseCases --> [EntityName]s --> Commands --> [SpecificUseCaseFolder]

![alt text](images\usecasefolder.png )

2. Create Response file. Name : [YourUsecase]CommandResponse

```csharp

public record UpdateSportVariantCommandResponse ();

```

3. Create Command file. Name [YourUsecase]Command.

```csharp
public record UpdateSportVariantCommand
    (
        Guid Id,
        string Name,
        string Description,
        string ImageUrl,
        bool IsOlympic,
        Guid SportTypeId,
        ScoringSystem RuleScoringSystem,
        int RulePlayerCount,
        int? RuleGameDuration,
        int? RuleMaxScore,
        IReadOnlyCollection<PopularInCountryDto> PopularInCountries,
        IReadOnlyCollection<PlayerPositionDto> PlayerPositions


    )
    : ICommand<Result<UpdateSportVariantCommandResponse>>;

```

4. Create Validator file. Name [YourUsecase]CommandValidator.

```csharp
internal sealed class UpdateSportVariantCommandValidator : AbstractValidator<UpdateSportVariantCommand>
    {
        public UpdateSportVariantCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithError(new Error("DescriptionNullOrEmpty", "Description Null or Empty"));

            RuleFor(x => x.Name).NotEmpty().WithError(new Error("NameNullOrEmpty", "Name Null or Empty"));

            RuleFor(x => x.SportTypeId).NotEmpty().WithError(new Error("SportTypeId", "SportTypeId Null or Empty"));
        }
    }

```

5. Create Handler file. Name [YourUseCase]CommandHandler

```csharp
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

             // Remove PopularInCountries if not exist in request
             var popularContries = sportVariant.PopularInCountries.Select(x => x.Id);
             var requestPopularCounties = request.PopularInCountries.Select(x => x.Id).Where(x => x.HasValue).Select(x => x.Value);

             var missingCountryIds = popularContries.Except(requestPopularCounties);

             foreach (var missingCountryId in missingCountryIds)
             {
                 sportVariant.RemovePopularInCountry(missingCountryId);
             }

             // Update sport variant properties
             if (request.PopularInCountries != null && request.PopularInCountries.Count != 0)
             {
                 foreach (var popularInCountries in request.PopularInCountries)
                 {
                     sportVariant.UpdatePopularInCountry(popularInCountries.Id, popularInCountries.CountryCode, popularInCountries.Popularity);
                 }
             }

             // Remove PlayerPositions if not exist in request
             var playerPositions = sportVariant.PlayerPositions.Select(x => x.Id);
             var requestPlayerPositions = request.PlayerPositions.Select(x => x.Id).Where(x => x.HasValue).Select(x => x.Value);
             var missingPlayerPositionIds = playerPositions.Except(requestPlayerPositions);

             foreach (var missingPlayerPositionId in missingPlayerPositionIds)
             {
                 sportVariant.RemovePlayerPosition(missingPlayerPositionId);
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
             _sportVariantRepository.Insert(sportVariant);


             await _unitOfWork.SaveChangesAsync(_requestContext.UserId, cancellationToken);
             _logger.LogInformation("Sport variant with name: {Name} created", request.Name);

             return Result.Success(new UpdateSportVariantCommandResponse());

         });
    }

```