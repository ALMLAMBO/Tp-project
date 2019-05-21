using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TpProject.Models.Data {
	[Table("tblChaptersForCourses")]
	public class ChaptersForCoursesDTO {
		[Key, Column(Order = 0)]
		public int CourseId { get; set; }
		[Key, Column(Order = 1)]
		public int ChapterId { get; set; }

		[ForeignKey("CourseId")]
		public virtual CourseDTO Course { get; set; }
		[ForeignKey("ChapterId")]
		public virtual ChapterDTO Chapter { get; set; }
	}
}