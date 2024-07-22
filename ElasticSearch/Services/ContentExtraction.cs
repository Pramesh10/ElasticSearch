
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;

using System.Text;

using ElasticSearch.Domain.Entities;
using Nest;

namespace ElasticSearch.Services
{
    public class ContentExtraction
    {
        public void tstFunction()
        {

            string folderPath = @"C:\Users\PrameshKumarKC\Source\Repos\Pramesh10\ElasticSearch\ElasticSearch\TestFiles";
            string[] filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
           
            foreach (var filePath in filePaths)
            {
                string content;
                string filename = System.IO.Path.GetFileName(filePath);

                switch (System.IO.Path.GetExtension(filePath))
                {
                    case ".pdf":
                        // Use a PDF parsing library (e.g., iTextSharp) to extract text content
                        content = ExtractTextFromPdf(filePath);
                        break;
                    case ".txt":
                        content = File.ReadAllText(filePath);
                        break;
                    //case ".xlsx":
                    //    // Use an Excel parsing library (e.g., EPPlus) to extract relevant data
                    //    content = ExtractDataFromExcel(filePath);
                    //    break;
                    default:
                        continue; // Skip unsupported file types
                }

                var settings = new Nest.ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("testdemo");

                var elasticClient = new Nest.ElasticClient(settings);

               
                    //var indexRequest = new IndexRequest<ESDocumentDetails>(
                    //    new ESDocumentDetails
                    //    {
                    //        Content = content,
                    //        FileName = filename,
                    //        // Add other document properties as needed
                    //    });
                    //var indexResponse = elasticClient.Index(indexRequest);

                  
                
                
               

                //if (!indexResponse.IsValid)
                //{
                //    // Handle any indexing errors
                //}

                var searchResponse = elasticClient.Search<ESDocumentDetails>(s => s
    .Query(q => q
        .Match(m => m // Match query
            .Field(f => f.Content) // Specify the field containing file content
            .Query("Divya Adhikari\r\n") // Replace with your search keywords
        )
    )
);

                Console.WriteLine(searchResponse);
            }

        }

        static string ExtractTextFromPdf(string path)
        {
            using (PdfReader pdfReader = new PdfReader(path))
            using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
            {
                StringBuilder text = new StringBuilder();

                for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page)));
                }

                return text.ToString();
            }
        }
    }
}
