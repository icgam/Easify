// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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