using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TpProject.Models.Data {
	public class Db : DbContext {
		public DbSet<PageDTO> Pages { get; set; }
		public DbSet<SidebarDTO> Sidebar { get; set; }
		public DbSet<CategoryDTO> Categories { get; set; }
		public DbSet<CourseDTO> Courses { get; set; }
		public DbSet<ChapterDTO> Chapters { get; set; }
		public DbSet<FileDTO> Files { get; set; }
		public DbSet<ChaptersForCoursesDTO> ChaptersForCourses { get; set; }
	}
}