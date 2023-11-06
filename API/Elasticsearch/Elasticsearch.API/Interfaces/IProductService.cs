using ElasticSearch.API.DTOs;
using ElasticSearch.API.Model;

namespace ElasticSearch.API.Interfaces
{
    public interface IProductService
    {
         Task SaveAsync();
    }
}
