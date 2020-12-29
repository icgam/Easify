using System;
using System.Threading.Tasks;
using FuManchu;

namespace Easify.Notification.Templating.HandleBars
{
    public sealed class HandleBarsTemplateContentRenderer : ITemplateContentRenderer
    {
        public HandleBarsTemplateContentRenderer()
        {
            Handlebars.RegisterHelper("date", options => $"{options.Data:dd/MM/yyyy}");
        }

        public Task<RenderedTemplate<T>> RenderAsync<T>(TemplateDefinition definition, T data) where T : class
        {
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            if (data == null) throw new ArgumentNullException(nameof(data));

            var template = Handlebars.CompileAndRun(definition.Name, definition.Template, data);
            return Task.FromResult(new RenderedTemplate<T>(template, data));
        }
    }
}