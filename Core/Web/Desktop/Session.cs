namespace Allors.Web.Desktop
{
    using System.Web.UI;

    public class Session : Control
    {
        private const string SessionKey = "Allors.Desktop.Page";

        private Window window;

        public Session()
        {
            this.ViewStateMode = ViewStateMode.Disabled;
            this.EnableViewState = false;
        }

        public Window Window
        {
            get
            {
                return this.window;
            }

            set
            {
                this.window = value;
                this.Controls.Clear();
                if (this.Window != null)
                {
                    this.Controls.Add(this.window);
                }
            }
        }

        public void Load()
        {
            this.Window = (Window)this.Page.Session[SessionKey];
        }

        public void Save()
        {
            this.Page.Session[SessionKey] = this.Window;
            this.Controls.Remove(this.Window);
        }

        protected override object SaveControlState()
        {
            return null;
        }

        protected override object SaveViewState()
        {
            return null;
        }
    }
}
