
using Codecaine.Common.Abstractions;
using Codecaine.Common.Persistence;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Codecaine.Implimentation.Tests.UnitTests.Apllication.UseCases.SportTypes.Commands.CreateSportType
{
    [TestFixture]
    internal class CreateSportTypeCommandHandlerTests
    {
        private Mock<ISportTypeRepository> _sportTypeRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IRequestContext> _requestContextMock;
        private Mock<ILogger<CreateSportTypeCommandHandler>> _loggerMock;
        private CreateSportTypeCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _sportTypeRepositoryMock = new Mock<ISportTypeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _requestContextMock = new Mock<IRequestContext>();
            _loggerMock = new Mock<ILogger<CreateSportTypeCommandHandler>>();
            var userId = "6B29FC40-CA47-1067-B31D-00DD010662DA";
            var userGuid = Guid.Parse(userId);

            _requestContextMock.Setup(r => r.UserId).Returns(userGuid);

            _handler = new CreateSportTypeCommandHandler(
                _sportTypeRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _requestContextMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenNameAlreadyExists()
        {
            // Arrange
            var command = new CreateSportTypeCommand("Football", "Team sport", "image.png");
            _sportTypeRepositoryMock.Setup(r => r.IsNameExistAsync(command.Name)).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess,Is.EqualTo(false));
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error.Code, Is.EqualTo("SportTypeNameExist"));
            _sportTypeRepositoryMock.Verify(r => r.Insert(It.IsAny<SportType>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Handle_ShouldCreateSportType_WhenNameDoesNotExist()
        {
            // Arrange
            var command = new CreateSportTypeCommand("Basketball", "Indoor team sport", "image2.png");

            _sportTypeRepositoryMock.Setup(r => r.IsNameExistAsync(command.Name)).ReturnsAsync(false);
            _sportTypeRepositoryMock.Setup(r => r.Insert(It.IsAny<SportType>()));
              

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.IsFailure, Is.EqualTo(false));

            _sportTypeRepositoryMock.Verify(r => r.Insert(It.IsAny<SportType>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
