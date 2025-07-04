﻿using Codecaine.Common.AiServices.Interfaces;
using Codecaine.Common.AiServices.OpenAi;
using Codecaine.Common.HttpServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Codecaine.Common.Tests.IntegrationTests.OpenAiServices
{
    [TestFixture]
    public class OpenAiEmbeddingServiceIntegrationTests
    {
        private IEmbeddingService _embeddingService;
        private IHttpClientFactory _factory;

        [SetUp]
        public void Setup()
        {
            // Load from environment or configuration
            var setting = new OpenAiSetting
            {
                BaseUrl = "https://api.openai.com/v1",
                ApiKey = "XXXXXXXXXXX_54fpSSuhSaru72sf_yu7fsZucVn3cw-5fGphyzc1gsMmIR4xUT3oMyw_vq44o9iZPtpT3BlbkFJ_6il9gSt6jECxsOhtLxBTf0TVmKCbqLUE5fgGjJSlBq5p5zzlHCe2F-jNKzdNL8qK-lc_XXXX",
                EmbeddingModel = "text-embedding-3-small"
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

            _embeddingService = new OpenAiEmbeddingService(options, httpService);
        }

        [Test]
        public async Task GetEmbeddingAsync_ShouldReturnEmbedding_ForValidInput()
        {
            // Arrange
            var input = "This is a test sentence for embedding.";

            // Act
            var embedding = await _embeddingService.GetVectorAsync(input);

            // Assert
            Assert.That(embedding,Is.Not.Null);
           
               
        }
    }
}
