namespace ElasticSearch.API.DTOs
{
    public record ProductUpdateRequestDTO(string Id, string Name, decimal Price,int Stock, ProductFeatureDTO Feature)
    {
    }
}
