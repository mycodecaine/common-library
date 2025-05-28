using Codecaine.Common.Persistence.MongoDB;
using Codecaine.Common.Persistence.MongoDB.Interfaces;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;

namespace Codecaine.SportService.Infrastructure.DataAccess.Repositories
{
    public class PlayerRepository : MongoRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(IMongoDbContext context) : base(context)
        {
        }

       
    }
}
