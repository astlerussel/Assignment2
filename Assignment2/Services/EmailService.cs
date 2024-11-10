namespace Assignment2.Services
{
    using Assignment2.Entities;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Policy;
    using System.Threading.Tasks;

    
        public class EmailService
        {
            public async Task SendEnrollmentConfirmation(string toEmail, string message)
            {
            


            var mailMessage = new MailMessage("astlerussel.test@gmail.com", toEmail)
                {
                    Subject = "Confirm Your Enrollment",
                    Body = message,
                    IsBodyHtml = true
                };

                using var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("astlerussel.test@gmail.com", "ctjs msiz lcav rkyj"),
                    EnableSsl = true,
                };
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }


