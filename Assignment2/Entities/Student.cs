using System.ComponentModel.DataAnnotations;

namespace Assignment2.Entities
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.ConfirmationMessageNotSent;

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }

    public enum EnrollmentStatus
    {
        ConfirmationMessageNotSent,
        ConfirmationMessageSent,
        EnrollmentConfirmed,
        EnrollmentDeclined
    }
}
