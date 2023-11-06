using ElasticSearch.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ElasticSearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDTO<T> response) 
        {

            if (response.StatusCode == HttpStatusCode.NoContent) //response bodysinin içi boşsa
                return new ObjectResult(null) { StatusCode = response.StatusCode.GetHashCode() };//response bodysinin içi boşsa body olarak null dön status code olarakta ne dönüyorsa onu dön

            return new ObjectResult(response) { StatusCode = response.StatusCode.GetHashCode()}; //response bodysinin içi boş değilse body olarak dönen response'u dön status code olarakta ne dönüyorsa onu dön

        }
    }
}
