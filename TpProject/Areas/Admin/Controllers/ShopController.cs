using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpProject.Models.Data;
using TpProject.Models.ViewModels.Shop;

namespace TpProject.Areas.Admin.Controllers {
    public class ShopController : Controller {
        // GET: Admin/Shop/Categories
        public ActionResult Categories() {
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
    }
}