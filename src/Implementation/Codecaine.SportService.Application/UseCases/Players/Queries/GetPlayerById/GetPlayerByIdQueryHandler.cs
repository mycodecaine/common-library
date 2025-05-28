using AutoMapper;
using Codecaine.Common.CQRS.Base;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.UseCases.SportTypes.Queries.GetSportTypeById;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.Players.Queries.GetPlayerById
{
   

    internal class GetPlayerByIdQueryHandler : QueryHandler<GetPlayerByIdQuery, Maybe<PlayerViewModel>>
    {
        private readonly ILogger<GetPlayerByIdQueryHandler> _logger;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        public GetPlayerByIdQueryHandler(ILogger<GetPlayerByIdQueryHandler> logger, IPlayerRepository playerRepository, IMapper mapper) : base(logger)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        public override async Task<Maybe<PlayerViewModel>> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken) =>
         await HandleSafelyAsync(async () =>
         {
             var playerResult = await _playerRepository.GetByIdAsync(request.Id);
             if (playerResult.HasNoValue)
             {
                 _logger.LogWarning("Player with id: {Id} not found", request.Id);
                 return Maybe<PlayerViewModel>.None;
             }

             var player = playerResult.Value;
             var playerViewModel = _mapper.Map<PlayerViewModel>(player);
             return playerViewModel;
         });
    }
}
