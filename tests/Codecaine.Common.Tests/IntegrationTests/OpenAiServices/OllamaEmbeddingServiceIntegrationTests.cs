using Codecaine.Common.AiServices.Interfaces;
using Codecaine.Common.AiServices.Ollama;
using Codecaine.Common.HttpServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Codecaine.Common.Tests.IntegrationTests.OpenAiServices
{
    internal class OllamaEmbeddingServiceIntegrationTests
    {
        private IEmbeddingService _embeddingService;
        private IHttpClientFactory _factory;

        [SetUp]
        public void Setup()
        {
            // Load from environment or configuration
            var setting = new OllamaSetting
            {
                BaseUrl = "http://localhost:11434/api",
                Model = "nomic-embed-text"
            };

            // Step 2: Mock IHttpClientFactory to return HttpClient
            var services = new ServiceCollection();
            services.AddHttpClient(); // registers IHttpClientFactory
            var provider = services.BuildServiceProvider();

            _factory = provider.GetRequiredService<IHttpClientFactory>();


            // Step 3: If you're using CreateClientWithPolicy(), mock an extension


            // Step 4: Mock ILogger
            var loggerMock = new Mock<ILogger<HttpService>>();

            var options = Options.Create(setting);
            var httpService = new HttpService(_factory, loggerMock.Object); // real implementation

            _embeddingService = new OllamaEmbeddingService(options, httpService);
        }

        [Test]
        public async Task GetEmbeddingAsync_ShouldReturnEmbedding_ForValidInput()
        {
            // Arrange
            var input = "heemi hanif test";

            // Act
            var embedding = await _embeddingService.GetVectorAsync(input);

            // Assert
            Assert.That(embedding, Is.Not.Null);


        }
    }
}
