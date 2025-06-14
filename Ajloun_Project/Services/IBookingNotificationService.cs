namespace Ajloun_Project.Services
{
    public interface IBookingNotificationService
    {
        void SendNotification(string toEmail, string subject, string body);
    }
}
