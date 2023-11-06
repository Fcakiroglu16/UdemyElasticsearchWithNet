using Elastic.Clients.Elasticsearch;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Interfaces;
using ElasticSearch.API.Model;
using ElasticSearch.API.Repository;
using Microsoft.Extensions.Logging;
//using Nest;
using System.Collections.Immutable;
using System.Net;

namespace ElasticSearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }


        public async Task<ResponseDTO<ProductDTO>> SaveAsync(ProductCreateRequestDTO request)
        {

            try
            {
                //tek tek burda maplemek yerine ProductCreateRequestDTO a gittik orda mapledik geldik.
                var responseProduct = await _productRepository.SaveAsync(request.CreateProduct());
                //aslında yukarda ProductRepository'den bize return'de ya null ya da newProduct dönüyo biz onu responseProduct'a setliyoruz.

                if (responseProduct == null) //Burası ProductRepository deki return'ün null dönme durumu için
                {
                    return ResponseDTO<ProductDTO>.Fail(new HashSet<string> { "kayıt esnasında bir sorun oluştu" },
                        System.Net.HttpStatusCode.InternalServerError);
                }

                return ResponseDTO<ProductDTO>.Succes(responseProduct.CreateDTO(), System.Net.HttpStatusCode.Created);
                //Burasıda ProductRepository deki return'ün newProduct dönme durumu için
            }
            catch (Exception)
            {

                throw;
            }



        }


        public async Task<ResponseDTO<List<ProductDTO>>> GetAllAsync()
        {

            try
            {
                var allProducts = await _productRepository.GetAllAsync();

                var allProductListDTO = new List<ProductDTO>();

                //var allProductList = allProducts.Select(p => new ProductDTO(p.Id, p.Name, p.Price, p.Stock, p.Create, p.Updated,
                //    new ProductFeatureDTO(p.Feature.Width, p.Feature.Height, p.Feature.Color))).ToList();


                foreach (var p in allProducts)
                {
                    if (p.Feature == null)
                    {
                        allProductListDTO.Add(new ProductDTO(p.Id, p.Name, p.Price, p.Stock, p.Create, p.Updated, null));
                    }
                    else
                    {
                        allProductListDTO.Add(new ProductDTO(p.Id, p.Name, p.Price, p.Stock, p.Create, p.Updated,
                            new ProductFeatureDTO(p.Feature.Width, p.Feature.Height, p.Feature.Color.ToString())));
                    }
                }

                return ResponseDTO<List<ProductDTO>>.Succes(allProductListDTO, HttpStatusCode.OK);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<ResponseDTO<ProductDTO>> GetByIdAsync(string id)
        {

            try
            {
                var hasProduct = await _productRepository.GetByIdAsync(id);

                if (hasProduct == null)
                {
                    return ResponseDTO<ProductDTO>.Fail("İlgili id ile Ürün bulunamadı.", HttpStatusCode.NotFound);
                }

                return ResponseDTO<ProductDTO>.Succes(hasProduct.CreateDTO(), HttpStatusCode.OK);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ResponseDTO<bool>> UpdateAsync(ProductUpdateRequestDTO updateProduct)
        {
            try
            {
                var isSucces = await _productRepository.UpdateAsync(updateProduct);

                if (isSucces)
                {
                    return ResponseDTO<bool>.Succes(true, HttpStatusCode.NoContent);
                }

                return ResponseDTO<bool>.Fail("Update sırasında bir hata oluştu.", HttpStatusCode.InternalServerError);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ResponseDTO<bool>> DeleteAsync(string id)
        {

            try
            {
                var deleteResponse = await _productRepository.DeleteAsync(id);

                #region Burası NEST kütüphanesi İLE OLAN
                //if (!deleteResponse.IsValid && deleteResponse.Result == Result.NotFound)
                //{
                //    return ResponseDTO<bool>.Fail("Silinecek ürün Database'de bulunamadı.", HttpStatusCode.NotFound);
                //}

                ////return ResponseDTO<bool>.Fail("Silme işlemi sırasında bir hata oluştu.", HttpStatusCode.InternalServerError);

                //if (!deleteResponse.IsValid)
                //{
                //    _logger.LogError(deleteResponse.OriginalException,deleteResponse.ServerError?.Error.ToString()); //Log tuttuk.Bu developer için önemli developerın hata için görmesi gereken

                //    return ResponseDTO<bool>.Fail("Silme işlemi sırasında bir hata oluştu.", HttpStatusCode.InternalServerError); // burasıda kullanıcının göreceği hata mesajı
                //}

                #endregion
                
                #region 

                if (!deleteResponse.IsValidResponse && deleteResponse.Result == Result.NotFound)
                {
                    return ResponseDTO<bool>.Fail("Silinecek ürün Database'de bulunamadı.", HttpStatusCode.NotFound);
                }

                //return ResponseDTO<bool>.Fail("Silme işlemi sırasında bir hata oluştu.", HttpStatusCode.InternalServerError);

                if (!deleteResponse.IsValidResponse)
                {
                    deleteResponse.TryGetOriginalException(out Exception? exception); //Burda exception'u aldık.Bir metotdan birden fazla nasıl geri dönebiliriz ? out,touble,sınıf,ref ile dönebiliriz. TryGetOriginalException geriye true dönerse exeption'un içerisini de data ile doldurur.
                    _logger.LogError(exception, deleteResponse.ElasticsearchServerError?.Error.ToString()); //Log tuttuk.Bu developer için önemli developerın hata için görmesi gereken

                    return ResponseDTO<bool>.Fail("Silme işlemi sırasında bir hata oluştu.", HttpStatusCode.InternalServerError); // burasıda kullanıcının göreceği hata mesajı
                }
                #endregion


                return ResponseDTO<bool>.Succes(true, HttpStatusCode.NoContent);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
