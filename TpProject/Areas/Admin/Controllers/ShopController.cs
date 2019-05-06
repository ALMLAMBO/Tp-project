using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
			if(!ModelState.IsValid) {
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
		public void ReorderCategories(int [] ids) {
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
				if(dto == null) {
					return Content("The category does not exists.");
				}

				model = new CategoryVM(dto);
			}
			return View(model);
		}

		// POST: Admin/Shop/RenameCategory/id
		[HttpPost]
		public ActionResult RenameCategory(CategoryVM model) {
			if(!ModelState.IsValid) {
				return View(model);
			}

			using (Db db = new Db()) {
				int id = model.Id;
				string slug = "";

				CategoryDTO dto = db.Categories.Find(id);
				dto.Name = model.Name;
				slug = model.Name.Replace(" ", "-").ToLower();

				if(db.Categories
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
	}
}