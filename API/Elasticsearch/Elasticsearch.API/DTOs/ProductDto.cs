using Elasticsearch.API.Models;
using Nest;
using System;

namespace Elasticsearch.API.DTOs
{
    public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
    {

      
      

        
       
    }
}
