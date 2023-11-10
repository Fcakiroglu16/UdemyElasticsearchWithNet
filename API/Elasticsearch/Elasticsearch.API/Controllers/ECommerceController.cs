using ElasticSearch.API.Models.ECommerceModel;
using ElasticSearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace ElasticSearch.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ECommerceController : BaseController
    {
        private readonly ECommerceRepository _eCommerceRepository; //Hiç bir zaman Controller'larınızı Repositorylere bağlamayın.
                                                                   //Eğer bağlarsanız Business'ı burda(Controllerda) yazmak zorunda kalırsınız.
                                                                   //Controller'lar Business yazacağınız yerler değildir.Controller'lar bir arabulucudur.
                                                                   //Requesti alır Responseı döner.

        public ECommerceController(ECommerceRepository eCommerceRepository)
        {
            _eCommerceRepository = eCommerceRepository;
        }

        [HttpGet("TermQuery{request}")]
        public async Task<IActionResult> TermQuery(string request)
        {
            var response = await _eCommerceRepository.TermQueryAsync(request);

            if (response == null)
            {
                return Ok(await _eCommerceRepository.TermQueryAsync(request));
            }

            return Ok(await _eCommerceRepository.TermQueryAsync(request));
        }


        [HttpPost("TermsQuery{requestList}")]
        public async Task<IActionResult> TermsQuery(List<string> requestList)
        {
            //var response = await _eCommerceRepository.TermsQuery(requestList);

            return Ok(_eCommerceRepository.TermsQueryAsync(requestList).Result);
        }



        [HttpGet("PrefixQuery/{request}")]
        public async Task<IActionResult> PrefixQuery(string request)
        {
            return Ok(await _eCommerceRepository.PrefixQueryAsync(request));
        }

        [HttpGet("RangeQuerey")]
        public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
        {
            return Ok(await _eCommerceRepository.RangeQueryAsync(fromPrice, toPrice));
        }

        [HttpGet("MatchAllQuery")]
        public async Task<IActionResult> MatchAllQuery()
        {
            return Ok(await _eCommerceRepository.MatchAllAsync());
        }


        [HttpGet("Pagination")]
        public async Task<IActionResult> Pagination(int page = 1, int pageSize = 10)
        {
            return Ok(await _eCommerceRepository.PaginationQueryAsync(page, pageSize));
        }

        [HttpGet("WildCardQuery")]
        public async Task<IActionResult> WildCardQuery(string request)
        {
            return Ok(await _eCommerceRepository.WildCardQuery(request));
        }

        [HttpGet("FuzzyQuery")]
        public async Task<IActionResult> FuzzyQuery(string request)
        {
            return Ok(await _eCommerceRepository.FuzzyQueryAsync(request));
        }

        [HttpGet("MatchQueryFullTextAsync")]
        public async Task<IActionResult> MatchQueryFullText(string request)
        {
            return Ok(await _eCommerceRepository.MatchQueryFullTextAsync(request));
        }

        [HttpGet("MatchBoolPrefixFullText")]
        public async Task<IActionResult> MatchBoolPrefixFullText(string request)
        {
            return Ok(await _eCommerceRepository.MatchBoolPrefixFullTextAsync(request));
        }

        [HttpGet("MatchPhraseFullText")]

        public async Task<IActionResult> MatchPhraseFullText(string request)
        {
            return Ok(await _eCommerceRepository.MatchPhraseFullTextAsync(request));
        }

        [HttpGet("CompoundQueryExampleOne")]
        public async Task<IActionResult> CompoundQueryExampleOne(string cityName, double taxtfulTotalPrice, string categoryName, string manufacturer)
        {
            return Ok(await _eCommerceRepository.CompoundQueryExampleOneAsync(cityName, taxtfulTotalPrice, categoryName, manufacturer));
        }
        [HttpGet("CompoundQueryExampleTwo")]
        public async Task<IActionResult> CompoundQueryExampleTwo(string customerFullName)
        {
            return Ok(await _eCommerceRepository.CompoundQueryExampleTwoAsync(customerFullName));

        }


        [HttpGet("MultiMatchQueryFullText")]
        public async Task<IActionResult> MultiMatchQueryFullText(string request)
        {
            return Ok(await _eCommerceRepository.MultiMatchQueryFullTextAsync(request));
        }
    }
}
