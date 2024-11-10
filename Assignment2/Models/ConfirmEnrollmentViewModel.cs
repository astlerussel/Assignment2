namespace Assignment2.Models
{
    public class ConfirmEnrollmentViewModel
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public string Room { get; set; }
        public DateTime StartDate { get; set; }
        public string Instructor { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
