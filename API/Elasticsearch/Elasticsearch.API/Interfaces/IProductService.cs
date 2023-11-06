using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;

namespace ElasticSearch.API.Interfaces
{
    public interface IProductService
    {
         Task SaveAsync();
    }
}
