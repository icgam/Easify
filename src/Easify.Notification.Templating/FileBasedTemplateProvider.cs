using System.IO;
using System.Threading.Tasks;
using Easify.Notification.Templating.Extensions;

namespace Easify.Notification.Templating
{
    public sealed class FileBasedTemplateProvider : ITemplateProvider
    {
        public Task<string> GetTemplateContentAsync(string templatePath)
        {
            var fullpath = templatePath.GetRelativePath();
            return Task.FromResult(File.ReadAllText(fullpath));
        }
    }
}