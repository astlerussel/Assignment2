namespace Assignment2.Services
{
    public interface IEmailService
    {
        Task SendEnrollmentConfirmation(string toEmail, string message, string subject);
    }
}
