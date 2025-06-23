using Codecaine.Common.AiServices.Interfaces;
using Codecaine.Common.Exceptions;
using Codecaine.Common.HttpServices;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Codecaine.Common.AiServices.Ollama
{
    public class OllamaEmbeddingService : IEmbeddingService
    {
        private readonly IHttpService _httpService;
        private readonly OllamaSetting _ollamaSetting;

        public OllamaEmbeddingService(IOptions<OllamaSetting> ollamaSetting,IHttpService httpService )
        {
            _httpService = httpService;
            _ollamaSetting = ollamaSetting.Value;
        }

        public async Task<List<float>> GetVectorAsync(string input)
        {
            var request = new
            {
                prompt = input,
                model = _ollamaSetting.Model
            };

            var ollamaUrl = $"{_ollamaSetting.BaseUrl}/embeddings";

            // Send a POST request to the token endpoint with the prepared request content
            var response = await _httpService.PostJsonAsync(ollamaUrl, request);
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<JsonDocument>(json);

            if (result == null || !result.RootElement.TryGetProperty("embedding", out var data) || data.GetArrayLength() == 0)
            {
                throw new CommonLibraryException(new Primitives.Errors.Error("FailedToRetrieveEmbbedingFromOllama", "Failed to retrieve embedding from Ollama."));
            }

            var embedding = data.EnumerateArray() .Select(e => e.GetSingle()) // Use GetDouble() if the model returns doubles
                    .ToList();

            return embedding;
        }
    }

    
}
