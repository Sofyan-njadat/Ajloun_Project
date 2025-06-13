using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Ajloun_Project.Services
{
    public class BookingNotificationService : IBookingNotificationService
    {
        private readonly SmtpSettings _smtpSettings;

        public BookingNotificationService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public void SendNotification(string toEmail, string subject, string body)
        {
            var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl
            };

            var mail = new MailMessage(_smtpSettings.FromEmail, toEmail, subject, body)
            {
                IsBodyHtml = false
            };

            client.Send(mail);
        }
    }
}
