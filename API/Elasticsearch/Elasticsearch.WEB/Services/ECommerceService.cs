using Elasticsearch.WEB.Repositories;
using Elasticsearch.WEB.ViewModels;

namespace Elasticsearch.WEB.Services
{
	public class ECommerceService
	{

		private readonly ECommerceRepository _repository;

		public ECommerceService(ECommerceRepository repository)
		{
			_repository = repository;
		}

		public async Task<(List<ECommerceViewModel>, long totalCount, long pageLinkCount)> SearchAsync(ECommerceSearchViewModel searchModel, int page, int pageSize)
		{
			var (eCommerceList, totalCount) = await _repository.SearchAsync(searchModel, page, pageSize);

			var pageLinkCountCalculate = totalCount % pageSize;

			long pageLinkCount = 0;

			if (pageLinkCountCalculate == 0)
			{
				pageLinkCount = totalCount / pageSize;
			}
			else
			{
				pageLinkCount = (totalCount / pageSize) + 1;

			}

			var eCommerceListViewModel = eCommerceList.Select(x => new ECommerceViewModel()
			{
				Category = String.Join(",", x.Category),
				CustomerFullName = x.CustomerFullName,
				CustomerFirstName = x.CustomerFirstName,
				CustomerLastName = x.CustomerLastName,
				OrderDate = x.OrderDate.ToShortDateString(),
				Gender = x.Gender.ToLower(),
				Id = x.Id,
				OrderId = x.OrderId,
				TaxfulTotalPrice = x.TaxfulTotalPrice



			}).ToList();


			return (eCommerceListViewModel, totalCount, pageLinkCount);



		}
	}
}
