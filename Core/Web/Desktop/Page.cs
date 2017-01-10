namespace Allors.Web.Desktop
{
    using System;
    using System.Linq;
    using System.Web.UI;

    public abstract class Page : System.Web.UI.Page
    {
        private const string SessionKey = "Allors.Desktop.Page";

        public Page()
        {
            this.ViewStateMode = ViewStateMode.Disabled;
            this.EnableViewState = false;
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (this.IsPostBack)
            {
                this.GetDesktopSession().Load();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            this.GetDesktopSession().Save();
        }

        protected override void SavePageStateToPersistenceMedium(object state)
        {
            // Ignore control- and viewstate.
        }

        protected override object SaveControlState()
        {
            return null;
        }

        protected override object SaveViewState()
        {
            return null;
        }

        protected abstract Session GetDesktopSession();
    }
}
