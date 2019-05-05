using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TpProject.Models.Data;

namespace TpProject.Models.ViewModels.Shop {
	public class CategoryVM {
		public CategoryVM() {

		}

		public CategoryVM(CategoryDTO row) {
			Id = row.Id;
			Name = row.Name;
			Slug = row.Slug;
			Sorting = row.Sorting;
		}

		public int Id { get; set; }
		[StringLength(int.MaxValue, MinimumLength = 2)]
		public string Name { get; set; }
		public string Slug { get; set; }
		public int Sorting { get; set; }
	}
}