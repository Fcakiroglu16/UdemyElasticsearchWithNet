using Elasticsearch.API.DTOs;
using Elasticsearch.API.Models;
using Elasticsearch.API.Repositories;
using Nest;
using System.Collections.Immutable;
using System.Net;

namespace Elasticsearch.API.Services
{
    public class ProductService
    {

        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {




            var responseProduct = await _productRepository.SaveAsync(request.CreateProduct());
            if (responseProduct == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "kayıt esnasında bir hata meydana geldi." }, System.Net.HttpStatusCode.InternalServerError);
            }



            return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), HttpStatusCode.Created);




        }


        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {


            var products = await _productRepository.GetAllAsync();
            var productListDto = new List<ProductDto>();



            foreach (var x in products)
            {

                if (x.Feature is null)
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, null));

                    continue;
                }


                productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature!.Height, x.Feature!.Color.ToString())));





            }



            return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);


        }


        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {

            var hasProduct = await _productRepository.GetByIdAsync(id);


            if (hasProduct == null)
            {
                return ResponseDto<ProductDto>.Fail("ürün bulunamadı", HttpStatusCode.NotFound);
            }

            var productDto = hasProduct.CreateDto();
           
            return ResponseDto<ProductDto>.Success(productDto, HttpStatusCode.OK);
        }



    }


}


