using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpProject.Models.Data;

namespace TpProject.Models.ViewModels.Shop {
	public class CourseVM {
		public CourseVM() {

		}

		public CourseVM(CourseDTO row) {
			Id = row.Id;
			Name = row.Name;
			Slug = row.Slug;
			Description = row.Description;
			Price = row.Price;
			CategoryName = row.CategoryName;
			CategoryId = row.CategoryId;
			VideoName = row.VideoName;
		}

		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Slug { get; set; }
		[Required]
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string CategoryName { get; set; }
		[Required]
		public int CategoryId { get; set; }
		public string VideoName { get; set; }

		public IEnumerable<SelectListItem> Categories { get; set; } 
	}
}