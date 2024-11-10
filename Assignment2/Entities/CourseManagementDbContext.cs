using Microsoft.EntityFrameworkCore;

namespace Assignment2.Entities
{
    public class CourseManagementDbContext : DbContext
    {
       
            public CourseManagementDbContext(DbContextOptions<CourseManagementDbContext> options) : base(options) { }

            public DbSet<Course> Courses { get; set; }
            public DbSet<Student> Students { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<Student>()
                .Property(s => s.Status)
                .HasConversion<string>(); // Maps enum to string in the database

            // Seed initial Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, Name = "Introduction to Programming", Instructor = "John Doe", StartDate = DateTime.Parse("2024-01-15"), RoomNumber = "1A01" },
                new Course { CourseId = 2, Name = "Advanced Mathematics", Instructor = "Jane Smith", StartDate = DateTime.Parse("2024-02-01"), RoomNumber = "2B02" }
            );

            // Seed initial Students associated with Courses using the EnrollmentStatus enum
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, Name = "Alice Johnson", Email = "alice.johnson@example.com", Status = EnrollmentStatus.ConfirmationMessageNotSent, CourseId = 1 },
                new Student { StudentId = 2, Name = "Bob Brown", Email = "bob.brown@example.com", Status = EnrollmentStatus.ConfirmationMessageNotSent, CourseId = 1 },
                new Student { StudentId = 3, Name = "Charlie Davis", Email = "charlie.davis@example.com", Status = EnrollmentStatus.ConfirmationMessageNotSent, CourseId = 2 }
            );
        }
        }
}
