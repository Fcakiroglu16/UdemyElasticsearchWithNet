using Elastic.Clients.Elasticsearch;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Interfaces;
using ElasticSearch.API.Model;
//using Nest;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repository
{
    public class ProductRepository
    {
        //private readonly ElasticClient _client; // Bu NEST kütüphanesindeki hali
        private readonly ElasticsearchClient _client; // Bu Elastic.Clients.ElasticSearch kütüphanesindeki hali
        
        private const string indexName = "products";

        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {

            newProduct.Create = DateTime.Now;
            var response = await _client.IndexAsync(newProduct, x => x.Index(indexName));// Burda eklediğimiz Id ElasticSearch Metadataya gidiyor.                                                                                                                          //ElasticSearch jargonunda Indexlemek demek datayı kaydetmek demektir.O yüzden Insert/Add/Save değil Index çıktı.

            //Fast Fail Yöntermi denir buna.Gereksiz if else'lerden kurtarır başarısız durumununu önce yazarız sonra başarılı durumunu yazarız.
            if (!response.IsValidResponse) //Elastic.Clients.ElasticSearch ile burda IsValid yok artık IsValidResponse var.
            {
                return null; //if false durumu
            }

            //if true durumu
            newProduct.Id = response.Id;
            return newProduct;

        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {

            var allProducts = await _client.SearchAsync<Product>(s => s.Index(indexName).Query(q => q.MatchAll()));

            foreach (var hit in allProducts.Hits)
            {
                hit.Source.Id = hit.Id; //Id bilgisi METADATA da Documents'in(Source) altında null geldiği için Hit'in altından alıp hit.Source.Id sine(YANİ Documents'in Id sine SETledim) SETledim. 
            }

            return allProducts.Documents.ToImmutableList();

        }

        public async Task<Product?> GetByIdAsync(string id)
        {

            var response = await _client.GetAsync<Product>(id, x => x.Index(indexName));

            if (!response.IsValidResponse)
            {
                return null;
            }
            response.Source.Id = response.Id;

            return response.Source;


        }

        public async Task<bool> UpdateAsync(ProductUpdateRequestDTO updateProduct)
        {
            #region Burası NEST kütüphanesi İLE OLAN
            //var response = await _client.UpdateAsync<Product, ProductUpdateRequestDTO>(updateProduct.Id,
            //x => x.Index(indexName).Doc(updateProduct));// Burası NEST kütüphanesi ile olan
            //return response.IsValid;
            #endregion

            #region Burası Elastic.Clients.ElasticSearch İLE OLAN

            var response = await _client.UpdateAsync<Product, ProductUpdateRequestDTO>(indexName, updateProduct.Id,
               x => x.Doc(updateProduct)); //İndexname'i,Updatelenecek olan datann Id sini ve o datanın yeni bilgilerini(nreProduct) ile verdik.

            return response.IsValidResponse;

            #endregion

        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {

            var response = await _client.DeleteAsync<Product>(id, x => x.Index(indexName));

            return response;
        }

    }



}




