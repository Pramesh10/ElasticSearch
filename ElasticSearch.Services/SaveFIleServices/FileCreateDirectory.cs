using ElasticSearch.Application.Abstraction;
using ElasticSearch.DataAccess.AppDbContexts;
using ElasticSearch.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Services.SaveFIleServices
{
    public class FileCreateDirectory : IFileCreateDirectory
    {

        private readonly string AppDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");

        public async Task<FileRecord> SaveFileToDir(IFormFile myFile)
        {
            FileRecord file = new FileRecord();
            if (myFile != null)
            {
                if (!Directory.Exists(AppDirectory))
                    Directory.CreateDirectory(AppDirectory);

                //var fileName =  myFile.FileName + Path.GetExtension(myFile.FileName);
                var fileName = myFile.FileName ;

                var path = Path.Combine(AppDirectory, fileName);

                file.Id = Guid.NewGuid();
                file.FilePath = path;
                file.FileName = fileName;
                file.FileFormat = Path.GetExtension(myFile.FileName);
                file.ContentType = myFile.ContentType;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await myFile.CopyToAsync(stream);
                }

                return file;
            }
            return file;
        }
    }
}
