using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpProject.Models.Data;
using TpProject.Models.ViewModels.Shop;

namespace TpProject.Controllers {
    public class ShopController : Controller {
        // GET: Shop
        public ActionResult Index() {
			return RedirectToAction("Index", "Pages");
		}

		[Route("Shop/CategoryMenuPartial")]
		public ActionResult CategoryMenuPartial() {

			List<CategoryVM> categoryVMList;
			using (Db db = new Db()) {
				categoryVMList = db.Categories
					.ToArray()
					.OrderBy(x => x.Sorting)
					.Select(x => new CategoryVM(x))
					.ToList();
			}

			return PartialView(categoryVMList);
		}

		// GET: /shop/category/name
		public ActionResult Category(string name) {
			List<CourseVM> courseVMList;

			using (Db db = new Db()) {
				CategoryDTO categoryDTO = db.Categories
					.Where(x => x.Slug == name)
					.FirstOrDefault();

				int catId = categoryDTO.Id;

				courseVMList = db.Courses
					.ToArray()
					.Where(x => x.CategoryId == catId)
					.Select(x => new CourseVM(x))
					.ToList();

				var courseCat = db.Courses
					.Where(x => x.CategoryId == catId)
					.FirstOrDefault();

				ViewBag.CategoryName = courseCat.CategoryName;
			}

			return View(courseVMList);
		}
	}
}