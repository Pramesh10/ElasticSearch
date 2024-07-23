
using ElasticSearch.Application.Abstraction;
using ElasticSearch.DataAccess.AppDbContexts;
using ElasticSearch.Domain.Entities;
using ElasticSearch.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.DataAccess.Repositories
{
    public class SaveFilesDetails : ISaveFilesDetails
    {

        private readonly AppDbContext _appDbContext;

        public SaveFilesDetails(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<DocumentDetail> SaveFilesDetailsToDb(FileRecord filetoCreate)
        {
            DocumentDetail fileData = new DocumentDetail
            {
                FilePath = filetoCreate.FilePath,
                FileName = filetoCreate.FileName,
                FileExtension = filetoCreate.FileFormat,
                MimeType = filetoCreate.ContentType,
                Id = Guid.NewGuid(),
                CreatedBy = "Admin",
                LastModified = DateTime.Now,
                CreatedDate = DateTime.Now,
                FileType = filetoCreate.ContentType
            };

            _appDbContext.DocumentDetails.Add(fileData);
            await _appDbContext.SaveChangesAsync();

            return fileData; 
        }
    }
}
