using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers
{
	public class ECommerceController : Controller
	{
		private readonly ECommerceService _service;

		public ECommerceController(ECommerceService service)
		{
			_service = service;
		}

		public async Task<IActionResult> Search([FromQuery] SearchPageViewModel searchPageView)
		{

			var (eCommerceList,totalCount,pageLinkCount) = await _service.SearchAsync(searchPageView.SearchViewModel, searchPageView.Page,
				searchPageView.PageSize);


			searchPageView.List = eCommerceList;
			searchPageView.TotalCount = totalCount;
			searchPageView.PageLinkCount = pageLinkCount;




			return View(searchPageView);
		}
	}
}
