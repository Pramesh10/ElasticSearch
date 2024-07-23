using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Domain.Models
{
    public class FileRecord
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileFormat { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public string AltText { get; set; }
        public string Description { get; set; }
    }
}
