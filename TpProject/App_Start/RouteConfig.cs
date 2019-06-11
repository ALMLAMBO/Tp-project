using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TpProject {
	public class RouteConfig {
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapMvcAttributeRoutes();

			routes.MapRoute(
				name: "Account", 
				url: "Account/{action}/{id}", 
				defaults: new { controller = "Account", action = "Index",
					id = UrlParameter.Optional }, 
				new[] { "TpProject.Controllers" }
			);

			routes.MapRoute(
				name: "Cart", 
				url: "Cart/{action}/{id}", 
				defaults: new { controller = "Cart", action = "Index",
					id = UrlParameter.Optional }, 
				new[] { "TpProject.Controllers" }
			);
			
			routes.MapRoute(
				name: "Shop", 
				url: "Shop/{action}/{name}", 
				defaults: new { controller = "Shop", action = "Index",
					name = UrlParameter.Optional }, 
				new[] { "TpProject.Controllers" }
			);
			
			routes.MapRoute(
				name: "SidebarPartial", 
				url: "Pages/SidebarPartial", 
				defaults: new { controller = "Pages", action = "SidebarPartial" }, 
				new[] { "TpProject.Controllers" }
			);

			routes.MapRoute(
				name: "PagesMenuPartial", 
				url: "Pages/PagesMenuPartial", 
				defaults: new { controller = "Pages", action = "PagesMenuPartial" },
				new[] { "TpProject.Controllers" }
			);

			routes.MapRoute(
				name: "Pages", 
				url: "{page}", 
				defaults: new { controller = "Pages", action = "Index" }, 
				new[] { "TpProject.Controllers" }
			);
			
			routes.MapRoute(
				name: "Default", 
				url: "", 
				defaults: new { controller = "Pages", action = "Index" }, 
				new[] { "TpProject.Controllers" }
			);

			//routes.MapRoute(
			//    name: "Default",
			//    url: "{controller}/{action}/{id}",
			//    defaults: new { controller = "Home", action = "Index",
			//		id = UrlParameter.Optional }
			//);
		}
	}
}
