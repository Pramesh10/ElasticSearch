using ElasticSearch.Domain.Entities;
using ElasticSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Application.Abstraction
{
    public interface ISaveFilesDetails
    {
        Task<DocumentDetail> SaveFilesDetailsToDb(FileRecord filetoCreate);

    }
}
