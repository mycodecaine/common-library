using Codecaine.Common.AiServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Codecaine.Common.AiServices.HuggingFace
{
    public class HuggingFaceEmbeddingService2 : IEmbeddingService
    {
        private const string Endpoint = "https://api-inference.huggingface.co/models/sentence-transformers/all-MiniLM-L6-v2";
        private readonly HttpClient _http;
        private readonly HuggingFaceSetting _settings;

        public HuggingFaceEmbeddingService2(IOptions<HuggingFaceSetting> settings,HttpClient http)
        {
            _settings = settings.Value;
            _http = http;
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
        }
        public async Task<List<float>> GetVectorAsync(string input)
        {
            var sentences = new[] {
           input
            
        };

            //var body = JsonSerializer.Serialize(new { inputs = sentences });

            var requestBody = new
            {
                inputs = sentences,
                options = new { wait_for_model = true }
            };

            var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json");

             var response = await _http.PostAsync(Endpoint, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var vectors = JsonConvert.DeserializeObject<float[][]>(json);
            var data = vectors[0];
            return data.ToList();
        }
    }
}
