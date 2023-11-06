using ElasticSearch.API.DTOs;
using ElasticSearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
    [Route("api/[controller]")] // bunlara artık ihtiyaç yok çünkü BaseControllerdan geliyor.
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(ProductCreateRequestDTO request)
        {
            return CreateActionResult(await _productService.SaveAsync(request)); //CreateActionResult bizden T(Gneric yapıda bir data bekliyor, parantezin içerisinde yazanlardan o datanın tipini çıkarabiliyor)
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await _productService.GetAllAsync());
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return CreateActionResult(await _productService.GetByIdAsync(id));

           
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ProductUpdateRequestDTO request)
        {
            return CreateActionResult(await _productService.UpdateAsync(request));
        }


        /// <summary>
        /// Hata yönetimi için  bu metod ele alınmıştır.
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return CreateActionResult(await _productService.DeleteAsync(id));
        }
    }
}
