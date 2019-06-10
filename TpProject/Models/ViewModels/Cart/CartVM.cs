using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TpProject.Models.ViewModels.Cart {
	public class CartVM {
		public int CourseId { get; set; }
		public string CourseName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Total { get { return Quantity * Price; } }
	}
}