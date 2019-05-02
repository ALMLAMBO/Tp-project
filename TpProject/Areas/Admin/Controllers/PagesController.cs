using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpProject.Models.Data;
using TpProject.Models.ViewModels.Pages;

namespace TpProject.Areas.Admin.Controllers {
    public class PagesController : Controller {
        // GET: Admin/Pages
        public ActionResult Index() {
			List<PageVM> pagesList;

			using (Db db = new Db()) {
				pagesList = db.Pages
					.ToArray()
					.OrderBy(x => x.Sorting)
					.Select(x => new PageVM(x))
					.ToList();
			}

			return View(pagesList);
        }

		// GET: Admin/Pages/AddPage
		[HttpGet]
		public ActionResult AddPage() {
			return View();
		}

		// POST: Admin/Pages/AddPage
		[HttpPost]
		public ActionResult AddPage(PageVM model) {
			if(!ModelState.IsValid) {
				return View(model);
			}

			using (Db db = new Db()) {
				string slug;
				PageDTO dto = new PageDTO();
				dto.Title = model.Title;

				if(string.IsNullOrWhiteSpace(model.Slug)) {
					slug = model.Title
						.Replace(" ", "-")
						.ToLower();
				}
				else {
					slug = model.Slug
						.Replace(" ", "-")
						.ToLower();
				}

				if(db.Pages.Any(x => x.Title == model.Title) 
					|| db.Pages.Any(x => x.Slug == slug)) {
					ModelState
						.AddModelError("", "That title or slug already exists");

					return View(model);
				}

				dto.Slug = slug;
				dto.Body = model.Body;
				dto.HasSidebar = model.HasSidebar;
				dto.Sorting = 100;

				db.Pages.Add(dto);
				db.SaveChanges();
			}

			TempData["SM"] = "You have added a new page";

			return RedirectToAction("AddPage");
		}

		// GET: Admin/Pages/EditPage/id
		[HttpGet]
		public ActionResult EditPage(int id) {
			PageVM model;

			using (Db db = new Db()) {
				PageDTO dto = db.Pages.Find(id);
				if(dto == null) {
					return Content("The page does not exists.");
				}

				model = new PageVM(dto);
			}
			return View(model);
		}

		// POST: Admin/Pages/EditPage/id
		[HttpPost]
		public ActionResult EditPage(PageVM model) {
			if(!ModelState.IsValid) {
				return View(model);
			}

			using (Db db = new Db()) {
				int id = model.Id;
				string slug = "home";

				PageDTO dto = db.Pages.Find(id);
				dto.Title = model.Title;
				if(model.Slug != "home") {
					if(string.IsNullOrWhiteSpace(model.Slug)) {
						slug = model.Title
							.Replace(" ", "-")
							.ToLower();
					}
					else {
						slug = model.Slug
							.Replace(" ", "-")
							.ToLower();
					}
				}

				if(db.Pages.Where(x => x.Id != id)
					.Any(x => x.Title == model.Title) 
					|| db.Pages.Where(x => x.Id != id)
					.Any(x => x.Slug == slug)) {

					ModelState
						.AddModelError("", "That title or slug already exists.");
				}

				dto.Slug = slug;
				dto.Body = model.Body;
				dto.HasSidebar = model.HasSidebar;

				db.SaveChanges();
			}
			TempData["SM"] = "You have edited the page!";

			return RedirectToAction("EditPage");
		}

		//GET: Admin/Pages/PageDetails/id
		public ActionResult PageDetails(int id) {
			PageVM model;

			using (Db db = new Db()) {
				PageDTO dto = db.Pages.Find(id);
				if(dto == null) {
					return Content("The page does not exist.");
				}
				model = new PageVM(dto);
			}
			return View(model);
		}
    }
}