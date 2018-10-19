using SIS.MVCFramework.ActionResults.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIS.MVCFramework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string,object> viewData)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewData = viewData;
        }

        public string Render()
        {
            var fullHtml = this.ReadFile();
            var renderedHtml = this.RenderHtml(fullHtml);

            return renderedHtml;
        }

        private string ReadFile()
        {
            if(!File.Exists(fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"View does not exists at {fullyQualifiedTemplateName}");
            }

            return File.ReadAllText(fullyQualifiedTemplateName);
        }

        private string RenderHtml(string fullHtml)
        {
            var renderedHtml = fullHtml;

            if(this.viewData.Any())
            {
                foreach (var param in this.viewData)
                {
                    renderedHtml = renderedHtml.Replace($"{{{{{param.Key}}}}}", param.Value.ToString());
                }
            }
            return renderedHtml;
        }
    }
}
