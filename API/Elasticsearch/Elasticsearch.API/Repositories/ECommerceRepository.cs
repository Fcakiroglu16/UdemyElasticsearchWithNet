using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.API.Models.ECommerceModel;
using System.Collections.Immutable;

namespace Elasticsearch.API.Repositories
{
    public class ECommerceRepository
    {

      

        private readonly ElasticsearchClient _client;

        public ECommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        private const string indexName = "kibana_sample_data_ecommerce";


        public  async Task<ImmutableList<ECommerce>>  TermQuery(string customerFirstName)
        {


            //1. way
            // var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));

            //2. way
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            //.Query(q => q.Term(t => t.CustomerFirstName.Suffix("keyword"), customerFirstName)));

            //3. way

            var termQuery= new TermQuery("customer_first_name.keyword") {  Value = customerFirstName, CaseInsensitive=true };

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termQuery));


            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();

        }
    }
}
