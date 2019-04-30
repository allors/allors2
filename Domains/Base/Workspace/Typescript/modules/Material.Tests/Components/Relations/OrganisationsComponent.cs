using Angular;

namespace src.allors.material.custom.relations.organisations
{
    using OpenQA.Selenium;

    using Angular.Html;

    public partial class OrganisationsComponent 
    {
        public Input Name => new Input(this.Driver, By.CssSelector($"input[formcontrolname='name']"));

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
