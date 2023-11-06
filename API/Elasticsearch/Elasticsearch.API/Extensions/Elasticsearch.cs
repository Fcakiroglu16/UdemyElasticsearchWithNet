using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
//using Elasticsearch.Net;
//using Nest;

namespace ElasticSearch.API.Extensions
{
    public static class ElasticsearchExt
    {
        public static void AddElasticService(this IServiceCollection services, IConfiguration configuration)
        {
            #region  NEST KÜTÜPHANESİ İLE OLAN KISIM

            //NEST KÜTÜPHANESİ İLE OLAN KISIM
            //var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            //// ! işareti ile diyoruz ki bana kızma bu data kesinlikle var bana kızma diyoruz.
            //var settings = new ConnectionSettings(pool);
            //var client = new ElasticClient(settings); // ElasticClient Treadsafe tir.Yani sen birden fazla tread'ten sen bu arkadaşa erişebilirsin.Multitread programin yapabilirsin.AMA Mesela EFCore daki DBContext sınıfımız treadsafe değildir.Örneğin DBContext'e farklı bir tread ten erişmek iştediğinde uygulama Exception fırlatır derki sen farklı bir tread'ten bu DBContext'e erişemezsin der.

            //services.AddSingleton(client); //Uygulama boyunca bir tane nesne üretilsin dedik.Bunu ElasticSearch'ün dokumanında böyle tavsiye ettiği için Singleton tanımladık her sınıfta böyle değil.

            #endregion

            #region Elastic.Clients.ElasticSearch KÜTÜPHANESİ İLE OLAN KISIM
            //Elastic.Clients.ElasticSearch KÜTÜPHANESİ İLE OLAN KISIM

            var userName = configuration.GetSection("Elastic")["Username"]; //Username bilgisini aldık
            var password = configuration.GetSection("Elastic")["Password"]; //Password bilgisini aldık
            var settings =  new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!))
                .Authentication(new BasicAuthentication(userName!, password!)); //Username ve Password ile Url de ki adrese bağlandık

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);

            #endregion





        }
    }
}
