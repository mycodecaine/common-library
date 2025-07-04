using Codecaine.Common.Persistence.Dapper;
using Codecaine.Common.Persistence.Dapper.Interfaces;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Dapper;


namespace Codecaine.SportService.Infrastructure.DataAccess.Repositories
{
    public class DocumentRepository : DapperRepository<Document>, IDocumentRepository
    {
        private readonly IDapperDbContext _context;

        public DocumentRepository(IDapperDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async  Task InsertAsync(Document document)
        {
            string sql = "INSERT INTO documents (id, content, embedding) VALUES (@Id, @Content, @Embedding)";

            await _context.Connection.ExecuteAsync(sql, new
            {
                Id = document.Id,
                Content = document.Content,
                Embedding = document.Embedding.ToArray()
            }, _context.Transaction);
         
        }

        public async Task<IEnumerable<(string Content, double Similarity)>> SearchSimilarAsync(List<float> queryEmbedding, int topK = 5)
        {
            
                var embeddingString = string.Join(",", queryEmbedding);  
                var sqlVector = @"
                        SELECT content,  (embedding <#> @Embedding::vector) AS similarity
                        FROM documents
                        ORDER BY embedding <#> @Embedding::vector
                        LIMIT @TopK;";

                return await _context.Connection.QueryAsync<(string Content, double Similarity)>(sqlVector, new
                {                    
                    Embedding = $"[{embeddingString}]",
                    TopK = topK
                });           
        }
    }
}
