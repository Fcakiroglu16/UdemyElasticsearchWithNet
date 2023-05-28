using Elasticsearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : ControllerBase
    {

        private readonly ECommerceRepository _repository;

        public ECommerceController(ECommerceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult>  TermQuery(string customerFirstName)
        {

            return Ok ( await _repository.TermQuery(customerFirstName));
        }


        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
        {

            return Ok(await _repository.TermsQuery(customerFirstNameList));
        }
    }
}
