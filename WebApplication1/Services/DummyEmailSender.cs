using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    public class DummyEmailSender : IEmailSender<User>
    {
        public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            Console.WriteLine($"[DummyEmailSender] Confirmation link: {confirmationLink}");
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            Console.WriteLine($"[DummyEmailSender] Password reset link: {resetLink}");
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            Console.WriteLine($"[DummyEmailSender] Password reset code: {resetCode}");
            return Task.CompletedTask;
        }
    }
}
