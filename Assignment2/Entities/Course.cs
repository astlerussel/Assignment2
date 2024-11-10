using System.ComponentModel.DataAnnotations;

namespace Assignment2.Entities
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Instructor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [RegularExpression(@"^\d[A-Z]\d{2}$", ErrorMessage = "Room number must be in format: single digit, capital letter, two digits (e.g., 3G15)")]
        public string RoomNumber { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
