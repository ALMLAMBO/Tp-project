using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpProject.Models.Data;
using TpProject.Models.ViewModels.Pages;

namespace TpProject.Controllers {
    public class PagesController : Controller {
        // GET: Index/{page}
        public ActionResult Index(string page = "") {
			if (page == "") {
				page = "home";
			}

			PageVM model;
			PageDTO dto;
			
			using (Db db = new Db()) {
				if (!db.Pages.Any(x => x.Slug.Equals(page))) {
					return RedirectToAction("Index", new { page = "" });
				}
			}

			using (Db db = new Db()) {
				dto = db.Pages
					.Where(x => x.Slug == page)
					.FirstOrDefault();
			}

			ViewBag.PageTitle = dto.Title;

			if (dto.HasSidebar == true) {
				ViewBag.Sidebar = "Yes";
			} 
			else {
				ViewBag.Sidebar = "No";
			}

			model = new PageVM(dto);

			return View(model);
        }
    }
}