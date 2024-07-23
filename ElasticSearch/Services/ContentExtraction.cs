
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;

using System.Text;

using ElasticSearch.Domain.Entities;
using Nest;
using ElasticSearch.Domain.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Functions;
using ClosedXML.Excel;

namespace ElasticSearch.Services
{
    public class ContentExtraction
    {
        

        public ContentExtraction()
        {
            
        }
        //        public void tstFunction()
        //        {

        //            string folderPath = @"C:\Users\PrameshKumarKC\Source\Repos\Pramesh10\ElasticSearch\ElasticSearch\TestFiles";
        //            string[] filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

        //            foreach (var filePath in filePaths)
        //            {
        //                string content;
        //                string filename = System.IO.Path.GetFileName(filePath);

        //                switch (System.IO.Path.GetExtension(filePath))
        //                {
        //                    case ".pdf":
        //                        // Use a PDF parsing library (e.g., iTextSharp) to extract text content
        //                        content = ExtractTextFromPdf(filePath);
        //                        break;
        //                    case ".txt":
        //                        content = File.ReadAllText(filePath);
        //                        break;
        //                    //case ".xlsx":
        //                    //    // Use an Excel parsing library (e.g., EPPlus) to extract relevant data
        //                    //    content = ExtractDataFromExcel(filePath);
        //                    //    break;
        //                    default:
        //                        continue; // Skip unsupported file types
        //                }

        //                var settings = new Nest.ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("testdemo");

        //                var elasticClient = new Nest.ElasticClient(settings);


        //                    //var indexRequest = new IndexRequest<ESDocumentDetails>(
        //                    //    new ESDocumentDetails
        //                    //    {
        //                    //        Content = content,
        //                    //        FileName = filename,
        //                    //        // Add other document properties as needed
        //                    //    });
        //                    //var indexResponse = elasticClient.Index(indexRequest);






        //                //if (!indexResponse.IsValid)
        //                //{
        //                //    // Handle any indexing errors
        //                //}

        //                var searchResponse = elasticClient.Search<ESDocumentDetails>(s => s
        //    .Query(q => q
        //        .Match(m => m // Match query
        //            .Field(f => f.Content) // Specify the field containing file content
        //            .Query("Divya Adhikari\r\n") // Replace with your search keywords
        //        )
        //    )
        //);

        //                Console.WriteLine(searchResponse);
        //            }

        //        }



        public async Task<FileRecord> ConvertToContent(FileRecord fileRecord)
        {
            Console.WriteLine(fileRecord);

            ElasticSearchSync eSsync = new ElasticSearchSync();


            string content;
            switch (fileRecord.FileFormat)
            {
                case ".pdf":
                    // Use a PDF parsing library (e.g., iTextSharp) to extract text content
                    content = ExtractTextFromPdf(fileRecord.FilePath);
                    eSsync.UploadContentToES(content, fileRecord.FileName);
                    break;
                case ".txt":
                    content = File.ReadAllText(fileRecord.FilePath);
                    eSsync.UploadContentToES(content, fileRecord.FileName);
                    break;
                case ".xlsx":
                    content = ExtractTextFromExcel(fileRecord.FilePath);
                    eSsync.UploadContentToES(content, fileRecord.FileName);
                    break;
                default:
                    Console.WriteLine("No match found");
                    break;
            }
            return await Task.FromResult( fileRecord);
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


        static string  ExtractTextFromExcel(string filePath)
        {
            XLWorkbook workbook = new XLWorkbook(filePath);
            var rowCount = workbook.Worksheet(1).LastRowUsed().RowNumber();
            var columnCount = workbook.Worksheet(1).LastColumnUsed().ColumnNumber();
            int column = 1;
            int row = 1;
            string content = "";
            while (row <= rowCount)
            {
                while (column <= columnCount)
                {
                    string title = workbook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                    
                    content += " " + title;
                    column++;
                }

                row++;
                column = 1;
            }
            Console.WriteLine(content);
            return content;

        }


        static async Task<string> ExtractTextAndIndexToElasticsearch(string filePath)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var rowCount = workbook.Worksheet(1).LastRowUsed().RowNumber();
                var columnCount = workbook.Worksheet(1).LastColumnUsed().ColumnNumber();

                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

                for (int row = 1; row <= rowCount; row++)
                {
                    var rowData = new Dictionary<string, object>();
                    for (int column = 1; column <= columnCount; column++)
                    {
                        rowData.Add($"Column{column}", workbook.Worksheets.Worksheet(1).Cell(row, column).GetString());
                    }
                    data.Add(rowData);
                }

                var settings = new Nest.ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("testdemo");

                var elasticClient = new Nest.ElasticClient(settings);

                var bulkResponse = await elasticClient.BulkAsync(b => b
                    .IndexMany(data)
                );

                if (!bulkResponse.IsValid)
                {
                    Console.WriteLine("Indexing failed: " + bulkResponse.ServerError);
                    return null;
                }
                else
                {
                    Console.WriteLine("Successfully indexed {0} rows to Elasticsearch", data.Count);
                    return "Data indexed successfully";
                }
            }
        }
    }
}


