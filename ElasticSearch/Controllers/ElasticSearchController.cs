using ElasticSearch.DataAccess.AppDbContexts;
using ElasticSearch.Services;
using Microsoft.AspNetCore.Http;
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
            ContentExtraction ce = new ContentExtraction();
            ce.tstFunction();
            var docDetails = await _appDbContext.DocumentDetails.ToListAsync();
            return Ok(docDetails);
        }
    }
}
