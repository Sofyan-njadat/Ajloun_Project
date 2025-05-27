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
                var smtpPort = int.Parse(smtpSettings["Port"]);
                var smtpUsername = smtpSettings["Username"];
                var smtpPassword = smtpSettings["Password"];
                var enableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true");
                var toEmail = "firasabumalloh@gmail.com";

                _logger.LogInformation("Preparing to send email using SMTP server: {Server}:{Port}", smtpServer, smtpPort);

                var message = new MailMessage
                {
                    From = new MailAddress(smtpUsername, smtpSettings["FromName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                };
                message.To.Add(toEmail);

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 10000 // 10 seconds timeout
                };

                _logger.LogInformation("Attempting to send email to {ToEmail} from {FromEmail}", toEmail, smtpUsername);
                
                try
                {
                    await client.SendMailAsync(message);
                    _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
                    return true;
                }
                catch (SmtpException smtpEx)
                {
                    _logger.LogError(smtpEx, "SMTP Error: {StatusCode} - {Message}", smtpEx.StatusCode, smtpEx.Message);
                    
                    // تحقق من رسالة الخطأ لمعرفة إذا كان الخطأ متعلق بالمصادقة
                    if (smtpEx.Message.Contains("authentication", StringComparison.OrdinalIgnoreCase) ||
                        smtpEx.Message.Contains("535"))
                    {
                        _logger.LogError("Authentication failed. Please check your username and app password.");
                    }
                    else if (smtpEx.StatusCode == SmtpStatusCode.MustIssueStartTlsFirst)
                    {
                        _logger.LogError("SSL/TLS is required but not enabled.");
                    }
                    else
                    {
                        _logger.LogError("SMTP error occurred while sending email.");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending email: {Message}", ex.Message);
                return false;
            }
        }
    }
} 