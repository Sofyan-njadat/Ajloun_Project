using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ajloun_Project.Services
{
    public interface IContactEmailService
    {
        Task<bool> SendContactEmailAsync(string subject, string body, string fromEmail, string fromName);
    }

    public class ContactEmailService : IContactEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactEmailService> _logger;

        public ContactEmailService(IConfiguration configuration, ILogger<ContactEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendContactEmailAsync(string subject, string body, string fromEmail, string fromName)
        {
            try
            {
                var smtp = _configuration.GetSection("SmtpSettings");
                var server = smtp["Server"];
                var port = int.Parse(smtp["Port"] ?? "587");
                var username = smtp["Username"];
                var password = smtp["Password"];
                var toEmail = smtp["FromEmail"]; // ثابت: بريد الإدارة
                var displayName = smtp["FromName"];
                var enableSsl = bool.Parse(smtp["EnableSsl"] ?? "true");

                var message = new MailMessage
                {
                    From = new MailAddress(username, displayName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                // لإمكانية الرد على المرسل
                if (!string.IsNullOrEmpty(fromEmail))
                {
                    message.ReplyToList.Add(new MailAddress(fromEmail, fromName));
                }

                using var client = new SmtpClient(server, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl,
                    Timeout = 30000
                };

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل إرسال الإيميل من صفحة Contact");
                return false;
            }
        }
    }
}
