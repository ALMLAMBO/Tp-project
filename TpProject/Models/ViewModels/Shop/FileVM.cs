using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TpProject.Models.Data;

namespace TpProject.Models.ViewModels.Shop {
	public class FileVM {
		public FileVM() {

		}

		public FileVM(FileDTO row) {
			Id = row.Id;
			Name = row.Name;
			FilePath = row.FilePath;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string FilePath { get; set; }
	}
}