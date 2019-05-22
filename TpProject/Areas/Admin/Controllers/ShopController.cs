using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using TpProject.Models.Data;
using TpProject.Models.ViewModels.Shop;

namespace TpProject.Areas.Admin.Controllers {
	public class ShopController : Controller {
		// GET: Admin/Shop/Index
		public ActionResult Index() {
			List<CategoryVM> categoriesVMList;
			using (Db db = new Db()) {
				categoriesVMList = db.Categories
					.ToArray()
					.OrderBy(x => x.Sorting)
					.Select(x => new CategoryVM(x))
					.ToList();
			}
			return View(categoriesVMList);
		}

		// GET: Admin/Shop/AddNewCategory
		[HttpGet]
		public ActionResult AddNewCategory() {
			return View();
		}

		// POST: Admin/Shop/AddNewCategory
		[HttpPost]
		public ActionResult AddNewCategory(CategoryVM model) {
			if (!ModelState.IsValid) {
				return View(model);
			}

			using (Db db = new Db()) {
				CategoryDTO dto = new CategoryDTO();
				dto.Name = model.Name;

				if (db.Categories
					.Any(x => x.Name == model.Name)) {
					ModelState
						.AddModelError("", "That category already exists.");

					return View(model);
				}

				dto.Slug = model.Name.Replace(" ", "-").ToLower();
				dto.Sorting = 100;

				db.Categories.Add(dto);
				db.SaveChanges();
			}
			TempData["SM"] = "You have added a new category!";

			return RedirectToAction("AddNewCategory");
		}

		// POST: Admin/Shop/ReorderCategories
		[HttpPost]
		public void ReorderCategories(int[] ids) {
			using (Db db = new Db()) {
				List<CategoryDTO> dtos = db.Categories.ToList();
				ids = new int[dtos.Count];

				for (int i = 0; i < dtos.Count; i++) {
					ids[i] = dtos[i].Id;
				}

				dtos = null;

				int count = 1;
				CategoryDTO dto;

				foreach (int catId in ids) {
					dto = db.Categories.Find(catId);
					dto.Sorting = count;
					db.SaveChanges();
					count++;
				}
			}
		}

		// GET: Admin/Shop/DeleteCategory/id
		public ActionResult DeleteCategory(int id) {
			using (Db db = new Db()) {
				CategoryDTO dto = db.Categories.Find(id);

				db.Categories.Remove(dto);
				db.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		// GET: Admin/Shop/RenameCategory/id
		[HttpGet]
		public ActionResult RenameCategory(int id) {
			CategoryVM model;
			using (Db db = new Db()) {
				CategoryDTO dto = db.Categories.Find(id);
				if (dto == null) {
					return Content("The category does not exists.");
				}

				model = new CategoryVM(dto);
			}
			return View(model);
		}

		// POST: Admin/Shop/RenameCategory/id
		[HttpPost]
		public ActionResult RenameCategory(CategoryVM model) {
			if (!ModelState.IsValid) {
				return View(model);
			}

			using (Db db = new Db()) {
				int id = model.Id;
				string slug = "";

				CategoryDTO dto = db.Categories.Find(id);
				dto.Name = model.Name;
				slug = model.Name.Replace(" ", "-").ToLower();

				if (db.Categories
					.Any(x => x.Name == model.Name)) {
					ModelState
						.AddModelError("", "The category name has been taken!");

					return View(model);
				}

				dto.Slug = slug;
				db.SaveChanges();
			}

			TempData["SM"] = "You have edited the category name!";

			return RedirectToAction("RenameCategory");
		}

		[HttpGet]
		// GET: Admin/Shop/AddCourse
		public ActionResult AddCourse() {
			CourseVM model = new CourseVM();

			using (Db db = new Db()) {
				model.Categories =
					new SelectList(db.Categories.ToList(), "Id", "Name");
			}

			return View(model);
		}

		// POST: Admin/Shop/AddCourse
		[HttpPost]
		public ActionResult AddCourse(CourseVM model
			, HttpPostedFileBase file) {

			if(!ModelState.IsValid) {
				using (Db db = new Db()) {
					model.Categories = 
						new SelectList(db.Categories.ToList(),
						"Id", "Name");

					return View(model);
				}
			}

			using (Db db = new Db()) {
				if(db.Courses
					.Any(x => x.Name == model.Name)) {

					model.Categories =
						new SelectList(db.Categories.ToList(),
						"Id", "Name");

					ModelState
						.AddModelError("", "That course name is taken!");

					return View(model);
				}
			}

			int id = 0;

			using (Db db = new Db()) {
				CourseDTO course = new CourseDTO();

				course.Name = model.Name;
				course.Slug = model.Name
					.Replace(" ", "-").ToLower();

				course.Description = model.Description;
				course.Price = model.Price;
				course.CategoryId = model.CategoryId;
				course.CreatedAt = DateTime.Now;

				CategoryDTO catDTO = db.Categories
					.FirstOrDefault(x => x.Id == model.CategoryId);

				course.CategoryName = catDTO.Name;

				db.Courses.Add(course);
				db.SaveChanges();

				id = course.Id;
			}

			#region Upload Video
				DirectoryInfo originalDirectory = 
				new DirectoryInfo(string.Format("{0}Courses\\", 
				Server.MapPath("~/")));

				string pathString1 = Path
					.Combine(originalDirectory.ToString() + "\\" + model.Name);

				string pathString2 = Path
					.Combine(originalDirectory.ToString() + 
					model.Name + "\\Chapters");

				if(!Directory.Exists(pathString1)) {
					Directory.CreateDirectory(pathString1);
				}

				if (!Directory.Exists(pathString2)) {
					Directory.CreateDirectory(pathString2);
				}

				if (file != null && file.ContentLength > 0) {
					string extension = file.ContentType.ToLower();
					if(extension != "video/mp4") {
						using (Db db = new Db()) {
							model.Categories = new SelectList(
								db.Categories.ToList(), "Id", "Name");

							ModelState
								.AddModelError("", "The video was not uploaded - wrong video format");

							return View(model);
						}
					}
				}

				string videoName = file.FileName;
				using (Db db = new Db()) {
					CourseDTO dto = db.Courses.Find(id);
					dto.VideoName = videoName;
					db.SaveChanges();
				}

				string path = string.Format("{0}\\{1}", pathString1, videoName);
				
				if(file.ContentLength < 999999999) {
					file.SaveAs(path);

					string mainconn = ConfigurationManager
						.ConnectionStrings["Db"]
						.ConnectionString;

					SqlConnection sqlconn = new SqlConnection(mainconn);
					string sqlquery =
						"UPDATE [dbo].[tblCourses] SET VideoName = @VideoName WHERE Id = @Id";

					SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);

					sqlconn.Open();
					sqlcomm.Parameters.AddWithValue("@VideoName", videoName);
					sqlcomm.Parameters.AddWithValue("@Id", id);
					sqlcomm.ExecuteNonQuery();
					sqlconn.Close();
				}
			#endregion

			TempData["SM"] = "You have added a new course!";

			return RedirectToAction("AddCourse");
		}

		//GET: Admin/Shop/Courses
		public ActionResult Courses(int? page, int? catId) {
			List<CourseVM> listOfCoursesVM;
			int pageNumber = page ?? 1;

			using (Db db = new Db()) {
				listOfCoursesVM = db.Courses
					.ToArray()
					.Where(x => catId == null 
						|| catId == 0 || x.CategoryId == catId)
					.Select(x => new CourseVM(x))
					.ToList();

				ViewBag.Categories = 
					new SelectList(db.Categories.ToList(), "Id", "Name");

				ViewBag.SelectedCat = catId.ToString();
			}

			IPagedList<CourseVM> onePageOfCourses 
				= listOfCoursesVM.ToPagedList(pageNumber, 3);

			ViewBag.OnePageOfCourses = onePageOfCourses;

			return View(listOfCoursesVM);
		}

		//GET: Admin/Shop/EditCourse/id
		[HttpGet]
		public ActionResult EditCourse(int id) {
			CourseVM model;

			using (Db db = new Db()) {
				CourseDTO dto = db.Courses.Find(id);

				if (dto == null) {
					return Content("The course does not exists.");
				}

				model = new CourseVM(dto);

				model.Categories = new SelectList(
					db.Categories.ToList(), "Id", "Name");
			}

			return View(model);
		}

		//POST: Admin/Shop/EditCourse/id
		[HttpPost]
		public ActionResult EditCourse(CourseVM model, 
			HttpPostedFileBase file) {

			int id = model.Id;

			using (Db db = new Db()) {
				model.Categories = 
					new SelectList(db.Categories.ToList(),
					"Id", "Name");
			}

			if(!ModelState.IsValid) {
				return View(model);
			}

			using (Db db = new Db()) {
				if(db.Courses.Where(x => x.Id != id)
					.Any(x => x.Name == model.Name)) {

					ModelState
						.AddModelError("", "The course name is taken!");

					return View(model);
				}
			}

			using (Db db = new Db()) {
				CourseDTO dto = db.Courses.Find(id);

				dto.Name = model.Name;
				dto.Slug = model.Name
					.Replace(" ", "-").ToLower();

				dto.Description = model.Description;
				dto.Price = model.Price;
				dto.CategoryId = model.CategoryId;
				dto.VideoName = model.VideoName;

				CategoryDTO catDTO = db.Categories
					.FirstOrDefault(x => x.Id == model.CategoryId);

				dto.CategoryName = catDTO.Name;

				db.SaveChanges();
			}

			TempData["SM"] = "You have edited the course!";

			#region Upload Video
				if(file != null && file.ContentLength > 0) {
					string extension = file.ContentType.ToLower();
					
					if(extension != "video/mp4") {
						ModelState
						.AddModelError("", 
						"The video was not uploaded - wrong video format!");

						return View(model);
					}

				DirectoryInfo originalDirectory =
				new DirectoryInfo(string.Format("{0}Courses\\",
				Server.MapPath("~/")));

				string pathString1 = Path
					.Combine(originalDirectory.ToString() +
						"\\" + model.Name);

				DirectoryInfo di1 = new DirectoryInfo(pathString1);

				foreach (FileInfo file2 in di1.GetFiles()) {
					file2.Delete();
				}

				string videoName = file.FileName;
				string path = string.Format("{0}\\{1}", pathString1, videoName);

				if (file.ContentLength < 999999999) {
					file.SaveAs(path);

					string mainconn = ConfigurationManager
						.ConnectionStrings["Db"]
						.ConnectionString;

					SqlConnection sqlconn = new SqlConnection(mainconn);
					string sqlquery =
						"UPDATE [dbo].[tblCourses] SET VideoName = @VideoName WHERE Id = @Id";

					SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);

					sqlconn.Open();
					sqlcomm.Parameters.AddWithValue("@VideoName", videoName);
					sqlcomm.Parameters.AddWithValue("@Id", id);
					sqlcomm.ExecuteNonQuery();
					sqlconn.Close();
				}
			}
			#endregion

			return RedirectToAction("EditCourse");
		}

		public ActionResult CourseDetails(int id) {
			CourseVM model;

			using (Db db = new Db()) {
				CourseDTO dto = db.Courses.Find(id);
				if(dto == null) {
					return Content("The course does not exitsts");
				}
				model = new CourseVM(dto);
			}

			return View(model);
		}

		//GET: Admin/Shop/DeleteCourse/id
		public ActionResult DeleteCourse(int id) {
			CourseVM model;

			using (Db db = new Db()) {
				CourseDTO dto = db.Courses.Find(id);
				model = new CourseVM(dto);

				db.Courses.Remove(dto);
				db.SaveChanges();
			}

			DirectoryInfo originalDirectory = 
				new DirectoryInfo(string.Format("{0}Courses\\",
				Server.MapPath("~/")));

			string pathString1 = Path
				.Combine(originalDirectory.ToString() 
				+ model.Name);

			string path = string
				.Format("{0}\\{1}", pathString1, model.VideoName);

			string pathString3 = Path
					.Combine(originalDirectory.ToString() +
					model.Name + "\\Chapters");

			if (Directory.Exists(pathString1)) {
				Directory.Delete(pathString1, true);
				DirectoryInfo info = new DirectoryInfo(pathString1);
				info.Delete();
			}
			 
			return RedirectToAction("Courses");
		}
	}
}