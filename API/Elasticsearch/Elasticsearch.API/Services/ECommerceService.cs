using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models.ECommerceModel;
using ElasticSearch.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace ElasticSearch.API.Services
{
    public class ECommerceService
    {
        private readonly ECommerceRepository eCommerceRepository;

        public ECommerceService(ECommerceRepository eCommerceRepository)
        {
            this.eCommerceRepository = eCommerceRepository;
        }


        //public async Task<ImmutableList<ECommerce>> TermQueryAsync(string customerFirstNameRequest)
        //{

        //    try
        //    {
        //        var result = await eCommerceRepository.TermQueryAsync(customerFirstNameRequest);

        //        if (result == null)
        //        return ResponseDTO<ECommerce>.Fail("İsim bulunamadı",HttpStatusCode.NotFound);
                

                
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
           
        //}
    }
}
