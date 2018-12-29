namespace Tests.Intranet.PartyRelationshipTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class PartyRelationshipEditPage : MainPage
    {
        public PartyRelationshipEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialDatePicker FromDate => new MaterialDatePicker(this.Driver, roleType: M.PartyRelationship.FromDate);

        public MaterialDatePicker ThroughDate => new MaterialDatePicker(this.Driver, roleType: M.PartyRelationship.ThroughDate);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

        public Button NewpartyRelationship => new Button(this.Driver, By.CssSelector("div[data-allors-class='partyrelationship']"));
    }
}
