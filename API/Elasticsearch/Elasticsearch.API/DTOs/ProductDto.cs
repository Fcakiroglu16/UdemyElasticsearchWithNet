using ElasticSearch.API.Models;
//using Nest;

namespace ElasticSearch.API.DTOs
{
    public record ProductDTO(string Id, string Name, decimal Price, int Stock, DateTime Create, DateTime? Updated, ProductFeatureDTO? Feature)
    {



    }
}

