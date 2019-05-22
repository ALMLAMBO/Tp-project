using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TpProject.Models.Data;

namespace TpProject.Models.ViewModels.Shop {
	public class ChapterVM {
		public ChapterVM() {

		}

		public ChapterVM(ChapterDTO row) {
			Id = row.Id;
			Name = row.Name;
			Description = row.Description;
			ChapterPath = row.ChapterPath;
		}

		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		public string ChapterPath { get; set; }
	}
}