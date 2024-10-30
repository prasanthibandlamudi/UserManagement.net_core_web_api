using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UserManagementSystem.Models;

public class EmailSenderService:IEmailSenderService
{
    private readonly IConfiguration _configuration;

    public EmailSenderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailNotification(User user, string plainTextPassword)
    {
        // Load SMTP settings from configuration
        var smtpSettings = _configuration.GetSection("SmtpSettings");
        var smtpClient = new SmtpClient
        {
            Host = smtpSettings["Host"],
            Port = int.Parse(smtpSettings["Port"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
            Credentials = new NetworkCredential(smtpSettings["FromEmail"], smtpSettings["AppPassword"])
        };
        var fromAddress = new MailAddress(smtpSettings["FromEmail"], smtpSettings["DisplayName"]);
        var toAddress = new MailAddress(user.Email, user.UserName);
        var subject = "Sharing Login Credentials";
        var body = $"Hello {user.UserName},\n\n" +
                   $"Your account has been created successfully.\n" +
                   $"Email: {user.Email}\n" +
                   $"Password: {plainTextPassword}\n" +
                   "Please log in at your earliest convenience.\n\n" +
                   "Thanks & Regards,\n" +
                   "Admin";
        var mailMessage = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        };
        await smtpClient.SendMailAsync(mailMessage);
    }
}
