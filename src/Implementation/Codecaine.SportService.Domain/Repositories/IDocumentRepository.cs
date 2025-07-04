using Codecaine.Common.Persistence.Dapper.Interfaces;
using Codecaine.SportService.Domain.Entities;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.Repositories
{
    public interface IDocumentRepository:IDapperRepository<Document>
    {
       // Task InsertAsync(Document document);
      //  Task<IEnumerable<(string Content, double Similarity)>> SearchSimilarAsync(List<float> queryEmbedding, int topK = 5);
    }
}
