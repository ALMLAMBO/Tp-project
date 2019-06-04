using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpProject.Models.ViewModels.Shop;
using TpProject.Models.Data;
using System.IO;

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

			if(!ModelState.IsValid) {
				return View(model);
			}

			using (Db db = new Db()) {
				if(db.Chapters
					.Any(x => x.Name == model.Name)) {

					ModelState
						.AddModelError("", "The chapter already exists!");

					return View(model);
				}
			}

			int id = 0;

			using (Db db = new Db()) {
				ChapterDTO chapter = new ChapterDTO();

				chapter.Name = model.Name;
				chapter.Description = model.Description;

				db.Chapters.Add(chapter);
				db.SaveChanges();

				id = chapter.Id;
			}

			#region Create Chapter
			DirectoryInfo originalDirectory = 
				new DirectoryInfo(string.Format("{0}Courses\\",
				Server.MapPath("~/")));

			CourseDTO course;

			using (Db db = new Db()) {
				ChaptersForCoursesDTO dto = new ChaptersForCoursesDTO() {
					CourseId = ShopController.getCourseId(id),
					ChapterId = id
				};

				course = db.Courses
					.Find(ShopController.getCourseId(id));

				db.ChaptersForCourses.Add(dto);
				db.SaveChanges();
			}

			string pathString = Path
				.Combine(originalDirectory.ToString() 
				+ "\\" + course.Name);

			string pathString2 = Path
				.Combine(pathString + "\\" + "Chapters");

			int chaptersCount = 0;

			using (Db db = new Db()) {
				chaptersCount = db
					.ChaptersForCourses
					.Where(x => x.CourseId == ShopController.getCourseId(id))
					.Count();
			}

			string pathString3 = Path
				.Combine(pathString2 + "\\" + string.Format(
					"Chapter " + chaptersCount.ToString() 
					+ " - " + model.Name));

			if(Directory.Exists(pathString3)) {
				Directory.CreateDirectory(pathString3);
			}

			#endregion

			TempData["SM"] = "You have added a new chapter!";

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

		//GET: Admin/Chapter/ChapterDetails/id
		public ActionResult ChapterDetails(int id) {
			return View();
		}

		public ActionResult DeleteChapter(int id) {
			return RedirectToAction("Chapters");
		}
    }
}