using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpProject.Models.Data;
using TpProject.Models.ViewModels.Account;

namespace TpProject.Controllers {
    public class AccountController : Controller {
        // GET: Account
        public ActionResult Index() {
            return Redirect("~/account/login");
        }

		// GET: account/create-account
		[ActionName("create-account")]
		[HttpGet]
		public ActionResult CreateAccount() {
			return View("CreateAccount");
		}

		// POST: account/create-account
		[ActionName("create-account")]
		[HttpPost]
		public ActionResult CreateAccount(UserVM model) {
			if (!ModelState.IsValid) {
				return View("CreateAccount", model);
			}

			if (!model.Password.Equals(model.ConfirmPassword)) {
				ModelState.AddModelError("", "Passwords do not match.");
				return View("CreateAccount", model);
			}

			using (Db db = new Db()) {
				if (db.Users.Any(x => x.Username.Equals(model.Username))) {
					ModelState.AddModelError("", "Username " + model.Username + " is taken.");
					model.Username = "";
					return View("CreateAccount", model);
				}

				UserDTO userDTO = new UserDTO() {
					FirstName = model.FirstName,
					LastName = model.LastName,
					EmailAddress = model.EmailAddress,
					Username = model.Username,
					Password = model.Password
				};

				db.Users.Add(userDTO);
				db.SaveChanges();

				int id = userDTO.Id;

				UserRoleDTO userRolesDTO = new UserRoleDTO() {
					UserId = id,
					RoleId = 2
				};

				db.UserRoles.Add(userRolesDTO);
				db.SaveChanges();
			}

			TempData["SM"] = "You are now registered and can login.";

			return Redirect("~/account/login");
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