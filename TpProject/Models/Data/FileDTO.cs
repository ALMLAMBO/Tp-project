
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TpProject.Models.Data {
	[Table("tblFiles")]
	public class FileDTO {
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string FilePath { get; set; }
	}
}