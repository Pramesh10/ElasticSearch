using DocumentFormat.OpenXml.Office2010.Word;
using ElasticSearch.Application.Abstraction;
using ElasticSearch.DataAccess.AppDbContexts;
using ElasticSearch.DataAccess.Repositories;
using ElasticSearch.Domain.Models;
using ElasticSearch.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net;

namespace ElasticSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        
        private readonly IFileCreateDirectory _fileCreateDirectory;
        private readonly ISaveFilesDetails _saveFileDetails;
        private readonly IDocuments _documentsRepo;
        private AppDbContext _appDbContext;


        private readonly string AppDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");


        public FilesController(IFileCreateDirectory fileCreateDirectory, ISaveFilesDetails saveFilesDetails, IDocuments documents
, AppDbContext appDbContext)
        {

            _fileCreateDirectory = fileCreateDirectory;
            _saveFileDetails = saveFilesDetails;
            _documentsRepo = documents;
            _appDbContext = appDbContext;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] FileModel model)
        {
            try{
                //save file to the directory 
                FileRecord file = await _fileCreateDirectory.SaveFileToDir(model.MyFile);

                if (!string.IsNullOrEmpty(file.FilePath))
                {
                    file.AltText = model.AltText;
                    file.Description = model.Description;
                
                    //Save to SQL Server DB
                    await _saveFileDetails.SaveFilesDetailsToDb(file);

                    ContentExtraction ce = new ContentExtraction();
                    await ce.ConvertToContent(file);
                    Console.WriteLine("hellotestasdfas");
                    return Ok(file);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        
        public async Task<IActionResult> DownloadFile(Guid id)
        {

            

            var doc = await _appDbContext.DocumentDetails.FirstOrDefaultAsync(doc => doc.Id == id);

            var path = Path.Combine(AppDirectory, doc?.FilePath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(path);

            await Console.Out.WriteLineAsync("hello");

            return File(memory, contentType, fileName);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> DownloadFiles(Guid id)
        {



            var doc = await _appDbContext.DocumentDetails.FirstOrDefaultAsync(doc => doc.Id == id);

            var path = Path.Combine(AppDirectory, doc?.FilePath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(path);

            return File(memory, contentType, fileName);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> DownloadFilessss(Guid id)
        {



            var doc = await _appDbContext.DocumentDetails.FirstOrDefaultAsync(doc => doc.Id == id);

            var path = Path.Combine(AppDirectory, doc?.FilePath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(path);

            return File(memory, contentType, fileName);
        }

    }
}
