using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Domain.Models
{
    public class FileModel
    {

        public IFormFile MyFile { get; set; }
        public string AltText { get; set; }
        public string Description { get; set; }
    }
}
