using System.Threading.Tasks;

namespace Easify.Notification.Templating
{
    public interface ITemplateContentRenderer
    {
        Task<RenderedTemplate<T>> RenderAsync<T>(TemplateDefinition definition, T data) where T : class;
    }
}