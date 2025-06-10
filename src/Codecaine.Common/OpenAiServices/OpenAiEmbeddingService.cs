using Codecaine.Common.Exceptions;
using Codecaine.Common.HttpServices;
using Codecaine.Common.OpenAiServices.Interfaces;
using Codecaine.Common.OpenAiServices.Utility;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Codecaine.Common.OpenAiServices
{
    /// <summary>
    /// If you need to use OpenAI embeddings, you can use this service.
    /// IF using Azure Open API embeddings, you can follow https://chatgpt.com/share/6846a247-fcfc-8007-98ef-65cb1a8bc4b1
    /// Take this notes to create OpenApiService https://chatgpt.com/share/6846a6bf-1cc0-8007-bcaa-d41562319c8b
    /// </summary>
    public class OpenAiEmbeddingService : IOpenAiEmbeddingService
    {
        private readonly OpenAiSetting _openAiSettings;
        private readonly IHttpService _httpService;

        public OpenAiEmbeddingService(IOptions<OpenAiSetting> openAiSettings, IHttpService httpService)
        {
            _openAiSettings = openAiSettings.Value;
            _httpService = httpService;
        }

        public async Task<List<float>> GetEmbeddingAsync(string input)
        {

            var tokenCount = TokenCounter.CountTokens(input);

            if (tokenCount > _openAiSettings.MaxTokens)
            {
                throw new CommonLibraryException(new Primitives.Errors.Error("InputTooLong", $"Input exceeds the maximum token limit of {_openAiSettings.MaxTokens}. Current token count: {tokenCount}."));
            }

            var request = new
            {
                input,
                model = _openAiSettings.Model
            };

            var openAiUrl = $"{_openAiSettings.BaseUrl}/embeddings";
                     

            // Send a POST request to the token endpoint with the prepared request content
            var response = await _httpService.PostJsonAsync(openAiUrl, request, _openAiSettings.ApiKey);

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<JsonDocument>(json);

            if (result == null || !result.RootElement.TryGetProperty("data", out var data) || data.GetArrayLength() == 0)
            {
                throw new CommonLibraryException( new Primitives.Errors.Error("FailedToRetrieveEmbbedingFromOpenAI", "Failed to retrieve embedding from OpenAI API."));
            }

            var embedding = result.RootElement
                .GetProperty("data")[0]
                .GetProperty("embedding")
                .EnumerateArray()
                .Select(e => e.GetSingle())
                .ToList();

            return embedding;
        }
    }
}
