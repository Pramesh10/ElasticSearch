using ElasticSearch.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Application.Abstraction
{
    public interface IFileCreateDirectory
    {
        Task<FileRecord> SaveFileToDir(IFormFile myFile);
    }
}
