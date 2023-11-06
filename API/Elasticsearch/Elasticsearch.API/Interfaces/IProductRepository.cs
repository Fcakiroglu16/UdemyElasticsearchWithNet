using ElasticSearch.API.DTOs;
using ElasticSearch.API.Model;

namespace ElasticSearch.API.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> SaveAsync(ProductCreateRequestDTO requestDTO);
    }
}
