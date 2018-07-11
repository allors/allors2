namespace Intranet.Pages
{
    using Allors.Meta;

    using Intranet.Tests;

    using PuppeteerSharp;
    using PuppeteerSharp.Input;

    public class PersonPage : BasePage
    {
        public PersonPage(Page page)
            : base(page)
        {
        }

        public MaterialSelect Salutation => new MaterialSelect(this.Page, roleType: M.Person.Salutation);

        public MaterialInput FirstName => new MaterialInput(this.Page, roleType: M.Person.FirstName);

        public MaterialInput MiddleName => new MaterialInput(this.Page, roleType: M.Person.MiddleName);

        public MaterialInput LastName => new MaterialInput(this.Page, roleType: M.Person.LastName);

        public Button Save => new Button(this.Page, selector: ".a-footer button[type='submit']");
    }
}
