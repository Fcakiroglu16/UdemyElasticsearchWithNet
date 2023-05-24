using Elasticsearch.Net;
using Nest;

namespace Elasticsearch.API.Extensions
{
    public static class ElasticsearchExt
    {

        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {

            var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));


            var settings = new ConnectionSettings(pool);


            var client = new ElasticClient(settings);

            services.AddSingleton(client);

        }
    }
}
