using ElasticSearch.Application.Abstraction;
using ElasticSearch.DataAccess.AppDbContexts;
using ElasticSearch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.DataAccess.Repositories
{
    public class DocumentRepository : IDocuments
    {

        private readonly AppDbContext _appDbContext;

        public DocumentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<DocumentDetail> GetDocumentByID(Guid documentId)
        {
            var f = await _appDbContext.DocumentDetails.ToListAsync();
            var doc =  await _appDbContext.DocumentDetails.FirstOrDefaultAsync(doc => doc.Id == documentId);
            return doc;
        }
    }
}
