using ElasticSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Application.Abstraction
{
    public interface IDocuments
    {
        Task<DocumentDetail> GetDocumentByID(Guid documentId);
    }
}
