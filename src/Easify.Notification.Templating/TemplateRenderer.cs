using System;
using System.Threading.Tasks;

namespace Easify.Notification.Templating
{
    public class TemplateRenderer : ITemplateRenderer
    {
        private readonly ITemplateContentRenderer _contentRenderer;
        private readonly ITemplateProvider _templateProvider;

        public TemplateRenderer(ITemplateProvider templateProvider, ITemplateContentRenderer contentRenderer)
        {
            _templateProvider = templateProvider ?? throw new ArgumentNullException(nameof(templateProvider));
            _contentRenderer = contentRenderer ?? throw new ArgumentNullException(nameof(contentRenderer));
        }

        public async Task<RenderedTemplate<T>> RenderTemplateAsync<T>(string name, string templatePath, T data)
            where T : class
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (templatePath == null) throw new ArgumentNullException(nameof(templatePath));
            if (data == null) throw new ArgumentNullException(nameof(data));

            var templateContent = await _templateProvider.GetTemplateContentAsync(templatePath);

            var templateDefinition = new TemplateDefinition(name, templateContent);
            return await _contentRenderer.RenderAsync(templateDefinition, data);
        }
    }
}