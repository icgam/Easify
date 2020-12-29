using System.Threading.Tasks;

namespace Easify.Notification.Templating
{
    public interface ITemplateProvider
    {
        Task<string> GetTemplateContentAsync(string templatePath);
    }
}