using Elasticsearch.API.Models;
using ElasticSearch.API.DTOs;
//using Nest;

namespace ElasticSearch.API.Models
{
    public class Product
    {
        //[PropertyName("_id")] //ElasticSearch taradında metadata tarafında tutulması için.ES tarafında bir PK id olarak tutulsun diye
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime Create { get; set; }
        public DateTime? Updated { get; set; }
        public ProductFeature? Feature { get; set; }


        public ProductDTO CreateDTO()
        {
            if (Feature == null)
            {
                return new ProductDTO(Id, Name, Price, Stock, Create, Updated, null); //Feature null olursa zaten Feature'yi dönmiycez
            }

            return new ProductDTO(Id, Name, Price, Stock, Create, Updated, new ProductFeatureDTO(Feature.Width, Feature.Height, Feature.Color.ToString()));
            //Burasıda Feature'un  null olmadğı case e feature da null değilse bu Model''in tüm bilgilerini dönersin bizde yukarda öyle yaptık
        }

    }
}
