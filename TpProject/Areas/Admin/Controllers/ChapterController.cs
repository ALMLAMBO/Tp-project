using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpProject.Models.ViewModels.Shop;
using TpProject.Models.Data;

namespace TpProject.Areas.Admin.Controllers {
    public class ChapterController : Controller {
		//GET: Admin/Chapter/Chapters
		public ActionResult Chapters() {
			return View();
		}
		
		//GET: Admin/Chapter/AddChapter
		[HttpGet]
		public ActionResult AddChapter() {
			return View();
		}

		//POST: Admin/Chapter/AddChapter
		[HttpPost]
		public ActionResult AddChapter(ChapterVM model, 
			HttpPostedFileBase file) {



			return RedirectToAction("AddChapter");
		}

		//GET: Admin/Chapter/EditChapter/id
		[HttpGet]
		public ActionResult EditChapter(int id) {
			return View();
		}

		//POST: Admin/Chapter/EditChapter/id
		[HttpPost]
		public ActionResult EditChapter(ChapterVM model, 
			HttpPostedFileBase file) {

			return RedirectToAction("Edit");
		}

		//GET: Admin/Chapter/ChapterDetailsr/id
		public ActionResult ChapterDetails(int id) {
			return View();
		}

		public ActionResult DeleteChapter(int id) {
			return RedirectToAction("Chapters");
		}
    }
}