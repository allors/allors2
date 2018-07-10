namespace Intranet.Pages
{
    using Intranet.Tests;

    using PuppeteerSharp;

    public class PeopleOverviewPage : BasePage
    {
        public PeopleOverviewPage(Page page)
            : base(page)
        {
        }

        public Input LastName
        {
            get
            {
                return new Input(this.Page, "input[formcontrolname='lastName']");
            }
        }
    }
}
