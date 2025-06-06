using Codecaine.Common.Domain;
using Codecaine.Common.Persistence.Dapper.Interfaces;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Infrastructure.DataAccess.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IDapperDbContext _context;

        public DocumentRepository(IDapperDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public  Task Insert(Document document)
        {
           // var sql = "SELECT * FROM Users WHERE Id = @Id";
          //  await _context.Connection.ExecuteAsync(sql, entity, _context.Transaction);

            throw new NotImplementedException("Insert method is not implemented yet.");
        }
    }
}
