using Codecaine.Common.Abstractions;
using Codecaine.Common.Exceptions;
using Codecaine.Common.Persistence;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.DTOs;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.UpdateSportVariant;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Enumerations;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Implimentation.Tests.UnitTests.Apllication.UseCases.SportVariants.Commands.UpdateSportVariant
{
    [TestFixture]
    internal class UpdateSportVariantCommandHandlerTests
    {
        private Mock<ISportVariantRepository> _sportVariantRepoMock;
        private Mock<ISportTypeRepository> _sportTypeRepoMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IRequestContext> _requestContextMock;
        private Mock<ILogger<UpdateSportVariantCommandHandler>> _loggerMock;
        private UpdateSportVariantCommandHandler _handler;


        [SetUp]
        public void Setup()
        {
            _sportVariantRepoMock = new Mock<ISportVariantRepository>();
            _sportTypeRepoMock = new Mock<ISportTypeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _requestContextMock = new Mock<IRequestContext>();
            _loggerMock = new Mock<ILogger<UpdateSportVariantCommandHandler>>();

            _handler = new UpdateSportVariantCommandHandler(
                _sportVariantRepoMock.Object,
                _sportTypeRepoMock.Object,
                _unitOfWorkMock.Object,
                _requestContextMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public void Handle_ShouldThrowNotFoundException_WhenSportTypeNotFound()
        {
            var request = CreateValidRequest();
            _sportTypeRepoMock.Setup(x => x.GetByIdAsync(request.SportTypeId)).ReturnsAsync(Maybe<SportType>.None);

            Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [Test]
        public void Handle_ShouldThrowNotFoundException_WhenSportVariantNotFound()
        {
            var request = CreateValidRequest();
            _sportTypeRepoMock.Setup(x => x.GetByIdAsync(request.SportTypeId)).ReturnsAsync(Maybe<SportType>.From(new SportType { }));
            _sportVariantRepoMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(Maybe<SportVariant>.None);

            Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenNameIsDuplicate()
        {
            var request = CreateValidRequest();

            _sportTypeRepoMock.Setup(x => x.GetByIdAsync(request.SportTypeId)).ReturnsAsync(Maybe<SportType>.From(new SportType { }));
            _sportVariantRepoMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(Maybe<SportVariant>.From(new SportVariant { }));
            _sportVariantRepoMock.Setup(x => x.IsDuplicateNameAsync(request.Id, request.SportTypeId, request.Name)).ReturnsAsync(true);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.That(result.IsFailure,Is.EqualTo(true));
            Assert.That(result.Error.Code,Is.EqualTo("SportVariantNameExist"));
        }


        private UpdateSportVariantCommand CreateValidRequest()
        {
            var command = new UpdateSportVariantCommand(
                    Id: Guid.NewGuid(),
            Name: "Street Soccer",
            Description: "A fast-paced variant of soccer played on streets or courts.",
            ImageUrl: "https://example.com/images/streetsoccer.png",
            IsOlympic: false,
            SportTypeId: Guid.NewGuid(),
            RuleScoringSystem: ScoringSystem.EliminationBased, // assuming enum
            RulePlayerCount: 5,
            RuleGameDuration: 30,
            RuleMaxScore: 10,
            PopularInCountries: new List<PopularInCountryDto>
                    {
                        new PopularInCountryDto
                            (
                            Id: Guid.NewGuid(),
                            CountryCode:  CountryCode.IE,
                            Popularity: 9
                            ),
                        new PopularInCountryDto
                            (
                            Id: Guid.NewGuid(),
                            CountryCode: CountryCode.CO,
                            Popularity: 7
                            )
                    },
            PlayerPositions: new List<PlayerPositionDto>
                    {
                        new PlayerPositionDto
                            (
                            Id: Guid.NewGuid(),
                            Name: "Striker",
                            Description: "Main goal scorer",
                            ImageUrl: "https://example.com/images/striker.png",
                            Responsibilities: "Score goals and press defense"
                            ),
                       new PlayerPositionDto(
                            Id: Guid.NewGuid(),
                            Name: "Defender",
                            Description: "Blocks opponent attacks",
                            ImageUrl: "https://example.com/images/defender.png",
                            Responsibilities: "Prevent goals and support goalkeeper"
                            )
                    }
                    );
            return command;
        }

    }
}
