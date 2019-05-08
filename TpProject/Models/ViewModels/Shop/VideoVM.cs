using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TpProject.Models.Data;

namespace TpProject.Models.ViewModels.Shop {
	public class VideoVM {
		public VideoVM() {

		}

		public VideoVM(VideoDTO row) {
			Id = row.Id;
			Name = row.Name;
		}

		public int Id { get; set; }
		public string Name { get; set; }
	}
}