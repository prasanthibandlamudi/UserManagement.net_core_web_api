using UserManagementSystem.Models;

public interface IEmailSenderService
{
    Task SendEmailNotification(User user, string plainTextPassword);
}
