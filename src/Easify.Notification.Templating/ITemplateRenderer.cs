using System.Threading.Tasks;

namespace Easify.Notification.Templating
{
    public interface ITemplateRenderer
    {
        Task<RenderedTemplate<T>> RenderTemplateAsync<T>(string name, string templatePath, T data) where T : class;
    }
}