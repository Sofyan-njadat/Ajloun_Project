using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ajloun_Project.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string subject, string body, string fromEmail, string fromName);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string subject, string body, string fromEmail, string fromName)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                // التحقق من وجود جميع الإعدادات المطلوبة
                if (string.IsNullOrEmpty(smtpSettings["Server"]) ||
                    string.IsNullOrEmpty(smtpSettings["Username"]) ||
                    string.IsNullOrEmpty(smtpSettings["Password"]))
                {
                    _logger.LogError("SMTP settings are incomplete. Please check appsettings.json");
                    return false;
                }

                var smtpServer = smtpSettings["Server"];
                var smtpPort = int.Parse(smtpSettings["Port"] ?? "587");
                var smtpUsername = smtpSettings["Username"]; // يجب أن يكون البريد الكامل
                var smtpPassword = smtpSettings["Password"];
                var enableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true");
                var adminEmail = smtpSettings["FromEmail"]; // البريد الذي ستصل إليه الرسائل
                var displayName = smtpSettings["FromName"];

                _logger.LogInformation("Preparing to send email using SMTP server: {Server}:{Port}", smtpServer, smtpPort);
                _logger.LogInformation("SMTP Username: {Username}", smtpUsername);

                var message = new MailMessage
                {
                    From = new MailAddress(smtpUsername, displayName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    Priority = MailPriority.Normal
                };

                // إرسال الرسالة إلى الإدارة
                message.To.Add(adminEmail);

                // إضافة المرسل الأصلي في Reply-To إذا كان موجوداً
                if (!string.IsNullOrEmpty(fromEmail) && IsValidEmail(fromEmail))
                {
                    message.ReplyToList.Add(new MailAddress(fromEmail, fromName ?? ""));
                }

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 30000 // زيادة timeout إلى 30 ثانية
                };

                _logger.LogInformation("Attempting to send email to {ToEmail} from {FromEmail}", adminEmail, smtpUsername);

                await client.SendMailAsync(message);
                _logger.LogInformation("Email sent successfully to {ToEmail}", adminEmail);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "SMTP Error: {StatusCode} - {Message}", smtpEx.StatusCode, smtpEx.Message);

                // تحليل مفصل لأخطاء SMTP
                switch (smtpEx.StatusCode)
                {
                    case SmtpStatusCode.GeneralFailure:
                        _logger.LogError("General SMTP failure. Check server settings.");
                        break;
                    case SmtpStatusCode.InsufficientStorage:
                        _logger.LogError("Insufficient storage on the server.");
                        break;
                    case SmtpStatusCode.ClientNotPermitted:
                        _logger.LogError("Client not permitted to send email.");
                        break;
                    case SmtpStatusCode.MustIssueStartTlsFirst:
                        _logger.LogError("Must enable SSL/TLS first.");
                        break;
                    default:
                        if (smtpEx.Message.Contains("authentication", StringComparison.OrdinalIgnoreCase) ||
                            smtpEx.Message.Contains("535") ||
                            smtpEx.Message.Contains("Username and Password not accepted"))
                        {
                            _logger.LogError("Authentication failed. Check Gmail App Password and username.");
                        }
                        else if (smtpEx.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
                        {
                            _logger.LogError("Connection timeout. Check network connection.");
                        }
                        else
                        {
                            _logger.LogError("SMTP error: {Message}", smtpEx.Message);
                        }
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending email: {Message}", ex.Message);
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}