using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.OpenAiServices.Interfaces
{
    public interface IOpenAiEmbeddingService
    {
        Task<List<float>> GetEmbeddingAsync(string input);
    }
}
