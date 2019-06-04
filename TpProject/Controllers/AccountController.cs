using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TpProject.Controllers {
    public class AccountController : Controller {
        // GET: Account
        public ActionResult Index() {
            return Redirect("~/account/login");
        }

		// GET: account/create-account
		[ActionName("create-account")]
		public ActionResult CreateAccount() {
			return View("CreateAccount");
		}

		// GET: /account/login
		public ActionResult Login() {
			string username = User.Identity.Name;

			if (!string.IsNullOrEmpty(username)) {
				return RedirectToAction("user-profile");
			}

			return View();
		}
	}
}