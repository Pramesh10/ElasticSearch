using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Domain.Entities
{
    public class DocumentDetail
    {
        [Key]
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public  string FileType { get; set; }
        public string FilePath{ get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public string CreatedBy{ get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
    
    }
}
