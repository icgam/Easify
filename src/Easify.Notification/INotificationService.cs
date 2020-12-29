using System.Threading.Tasks;
using Easify.Notification.Configuration;

namespace Easify.Notification
{
    public interface INotificationService
    {
        Task SendNotificationAsync<T>(Notification<T> notification,
            string profileName = NotificationProfileNames.DefaultProfile) where T : class;
    }
}