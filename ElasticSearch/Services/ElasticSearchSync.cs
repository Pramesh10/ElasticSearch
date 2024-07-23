using ElasticSearch.Domain.Entities;
using Nest;

namespace ElasticSearch.Services
{
    public class ElasticSearchSync
    {

        public ElasticSearchSync() { }

        public void UploadContentToES(string content , string filename)
        {

            var settings = new Nest.ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("testdemo");

            var elasticClient = new Nest.ElasticClient(settings);


            var indexRequest = new IndexRequest<ESDocumentDetails>(
                new ESDocumentDetails
                {
                    Content = content,
                    FileName = filename,
                    // Add other document properties as needed
                });
            var indexResponse = elasticClient.Index(indexRequest);
            Console.WriteLine(indexResponse);
            if (!indexResponse.IsValid)
            {
                throw new Exception();
            }
            else
            {
                Console.WriteLine("Synced Successfully");
            }
        }
    }
}
