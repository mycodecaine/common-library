using Codecaine.Common.AiServices.Interfaces;
using Codecaine.Common.AiServices.Model;
using Codecaine.Common.AiServices.OpenAi;
using Codecaine.Common.HttpServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Tests.IntegrationTests.OpenAiServices
{
    [TestFixture]
    internal class OpenAiLargeLanguageModelServiceTests
    {
       
        private IHttpClientFactory _factory;
        private ILargeLanguageModelService _llmService;

        [SetUp]
        public void Setup()
        {
            // Load from environment or configuration
            var setting = new OpenAiSetting
            {
                BaseUrl = "https://api.openai.com/v1",
                ApiKey = "XXX_54fpSSuhSaru72sf_yu7fsZucVn3cw-5fGphyzc1gsMmIR4xUT3oMyw_vq44o9iZPtpT3BlbkFJ_6il9gSt6jECxsOhtLxBTf0TVmKCbqLUE5fgGjJSlBq5p5zzlHCe2F-jNKzdNL8qK-lc_daU4A",
                LargeLanguageModel = "gpt-3.5-turbo"
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

            _llmService = new OpenAiLargeLanguageModelService(options, httpService);
        }

        [Test]
        public async Task GenerateTextAsync_ShouldReturnText_ForValidInput()
        {
            // Arrange
            var prompt = new PromptMessage
            (
                 "user",
                 "What is the capital of France?"
            );
            // Act
            var response = await _llmService.GenerateTextAsync(new List<PromptMessage> { prompt});
            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.Not.Empty);
            Assert.That(response, Does.Contain("Paris"));
        }

    }
}
