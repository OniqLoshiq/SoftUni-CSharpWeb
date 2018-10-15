using SIS.MVCFramework.ActionResults.Contracts;
using System.IO;

namespace SIS.MVCFramework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName; 
        }

        private string ReadFile(string fullyQualifiedTemplateName)
        {
            if(!File.Exists(fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"View does not exists at {fullyQualifiedTemplateName}");
            }

            return File.ReadAllText(fullyQualifiedTemplateName);
        }

        public string Render()
        {
            var fullHtml = this.ReadFile(this.fullyQualifiedTemplateName);

            return fullHtml;
        }
    }
}
