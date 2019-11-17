namespace Allors.Workspace.Blazor.Html
{
    using Markdig;

    public class AMarkdownHtmlBase : RoleField
    {
        public string Html
        {
            get
            {
                var model = this.Model as string;
                return !string.IsNullOrEmpty(model) ? Markdown.ToHtml(model) : string.Empty;
            }
        }
    }
}
