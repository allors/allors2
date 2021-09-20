namespace Allors.Workspace.Blazor.Forms.Roles
{
    using Markdig;

    public class AMarkdownHtmlBase : RoleField
    {
        public string Html
        {
            get
            {
                var model = this.Model as string;
                var html = !string.IsNullOrEmpty(model) ? Markdown.ToHtml(model) : string.Empty;
                return html;
            }
        }
    }
}
