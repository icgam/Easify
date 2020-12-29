using System.Threading.Tasks;

namespace Easify.Notification.Messaging
{
    public interface IMessagingService
    {
        Task SendAsync(Message message);
    }
}