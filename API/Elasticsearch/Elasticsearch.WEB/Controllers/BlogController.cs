using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers
{
	public class BlogController : Controller
	{

		private BlogService _blogService;

		public BlogController(BlogService blogService)
		{
			_blogService = blogService;
		}


		public async Task<IActionResult> Search()
		{


			return View(await _blogService.SearchAsync(string.Empty));
		}
		[HttpPost]
		public async Task<IActionResult> Search(string searchText )
		{

			ViewBag.searchText = searchText;

			return View(await _blogService.SearchAsync(searchText));
		}

        public IActionResult Save()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Save(BlogCreateViewModel model)
		{

			var isSucces = await _blogService.SaveAsync(model);

			if (!isSucces)
			{
				TempData["result"] = "kayıt başarısız";
				return RedirectToAction(nameof(BlogController.Save));
			}


			TempData["result"] = "kayıt başarılı";
			return RedirectToAction(nameof(BlogController.Save));
		}


	}
}
