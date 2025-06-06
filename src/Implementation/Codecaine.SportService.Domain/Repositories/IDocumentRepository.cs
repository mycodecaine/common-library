using Codecaine.SportService.Domain.Entities;

namespace Codecaine.SportService.Domain.Repositories
{
    public interface IDocumentRepository
    {
        Task Insert(Document document);
    }
}
