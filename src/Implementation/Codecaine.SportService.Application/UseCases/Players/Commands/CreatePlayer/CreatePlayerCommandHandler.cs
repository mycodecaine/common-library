using Codecaine.Common.Abstractions;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Persistence;
using Codecaine.Common.Persistence.MongoDB.Interfaces;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Codecaine.SportService.Application.UseCases.Players.Commands.CreatePlayer
{
    internal sealed class CreatePlayerCommandHandler : CommandHandler<CreatePlayerCommand, Result<CreatePlayerCommandResponse>>
    {
        private readonly ILogger<CreatePlayerCommandHandler> _logger;
        private readonly IPlayerRepository _playerRepository;
        private readonly INoSqlUnitOfWork _unitOfWork;
        private readonly IRequestContext _requestContext;
        public CreatePlayerCommandHandler(ILogger<CreatePlayerCommandHandler> logger, IPlayerRepository playerRepository , INoSqlUnitOfWork unitOfWork, IRequestContext requestContext) : base(logger)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
            _requestContext = requestContext;
        }

        public override  async Task<Result<CreatePlayerCommandResponse>> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        =>
         await HandleSafelyAsync(async () =>
         {
             

             var player = Player.Create(request.Name, request.Description, request.ImageUrl);

             _logger.LogInformation("Creating sport type with name: {Name}", request.Name);
             await _unitOfWork.StartTransactionAsync(_requestContext.UserId, cancellationToken);
             _playerRepository.Insert(player);


             await _unitOfWork.CommitAsync(player);
             _logger.LogInformation("Sport type with name: {Name} created", request.Name);

             return Result.Success(new CreatePlayerCommandResponse(player.Id));

         });
    }
}
