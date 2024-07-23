using ElasticSearch.Application.Abstraction;
using ElasticSearch.DataAccess.AppDbContexts;
using ElasticSearch.Domain.Models;
using ElasticSearch.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    


        public FilesController(IFileCreateDirectory fileCreateDirectory,ISaveFilesDetails saveFilesDetails
            )
        {
           
            _fileCreateDirectory = fileCreateDirectory;
            _saveFileDetails = saveFilesDetails;
          
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
    }
}
