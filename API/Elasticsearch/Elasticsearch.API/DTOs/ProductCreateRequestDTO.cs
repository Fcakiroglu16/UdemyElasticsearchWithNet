using Elasticsearch.API.Models;
using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs
{
    public record ProductCreateRequestDTO(string Name, decimal Price, int Stock, ProductFeatureDTO Feature)
    {
        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature()
                {
                    Width = Feature.Width,
                    Height = Feature.Height,
                    Color =(EColor)int.Parse(Feature.Color)//Bir stringi Enuma çevirmek için onu önce inte çeviririz sonra Enuma çeviririz.
                }
            };

        }
    };
}


//record:Bunlar referans tipleri gibidir ama bunlar IMutable olarak kullanılabilir.
//Yani bu classtan bir nesne örneği ürettiğimiz zaman onu üretilen nesne örneğinin propertylerini değiştiremiyoruz bu öenmli bir konu
//Yani bir nesnesin üretildikten sonra propertylerinin  değiştirelememsi(IMutable olması yani) bir çok hatayı önler.
//ÖR: Bir classımız var bı sınıf 4 farklı Microserviste dolanıyo(requestin bodysine gönderiyoruz yani.Bunun tipi class olursa
//Bu Microservislerde bu nesnenin bir propertysi değiştirildi diyelim bunu bulmak çok zordur o yüzde record olması yani değiştirelemez(IMutable) olması daha sağlıklı.
//Record tanımlanan bir nesne değiştirilmeye kalkıldığında hata verir bizde hangi yerde değiştirilmeye çalıştığını kolay bi şekilde anlarız.