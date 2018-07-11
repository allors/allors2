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

        public Input LastName => new Input(this.Page, formControlName: "lastName");
    }
}
