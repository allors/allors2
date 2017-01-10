namespace WebApplication
{
    using System;
    using Allors.Web.Desktop;

    using Page = Allors.Web.Desktop.Page;

    public partial class Default : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var window = (Window)this.LoadControl("~/Controls/DefaultWindow.ascx");
                this.DesktopSession.Window = window;
            }

            base.OnLoad(e);
        }

        protected override Session GetDesktopSession()
        {
            return this.DesktopSession;
        }
    }
}