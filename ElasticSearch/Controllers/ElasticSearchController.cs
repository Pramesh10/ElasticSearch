using ElasticSearch.DataAccess.AppDbContexts;
using ElasticSearch.Domain.Entities;
using ElasticSearch.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElasticSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticSearchController : ControllerBase
    {
        private AppDbContext _appDbContext;
        public ElasticSearchController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        [HttpGet("GetAllDocumentsDetails")]
        public async Task<IActionResult> GetAllDocumentsDetails()
        {
            
            var docDetails = await _appDbContext.DocumentDetails.ToListAsync();
            return Ok(docDetails);
        }


        [HttpGet("SearchWithContent")]
        public async Task<IActionResult> SearchWithContent([FromQuery] string searchTerm)
        {
           
           var settings = new Nest.ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("contentsearch");

           var elasticClient = new Nest.ElasticClient(settings);

            var searchResponse = elasticClient.Search<ESDocumentDetails>(s => s
                            .Query(q => q
                             .Match(m => m 
                               .Field(f => f.Content) 
                                  .Query(searchTerm) 
                                         )
                             )
                );

            List<DocumentDetail> fileList = new List<DocumentDetail>();

            foreach (var item in searchResponse.Documents)
            {
                Console.WriteLine(item.FileName);
                var docDetails = await _appDbContext.DocumentDetails.Where(i => i.FileName == item.FileName).FirstOrDefaultAsync();
                var doc = new DocumentDetail
                {
                    FileName = item.FileName,
                    Id = docDetails.Id,
                    FilePath = docDetails.FilePath,
                    FileExtension = docDetails.FileExtension,
                    MimeType = docDetails.MimeType,
                    CreatedBy = docDetails.CreatedBy,
                    CreatedDate = docDetails.CreatedDate
                };

                fileList.Add(doc);
            }
            return Ok(fileList);
        }
    }
}
