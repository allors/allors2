using src.app.main;

namespace Pages.PartyRelationshipTests
{
    using Allors.Meta;
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PartyRelationshipEditComponent : MainComponent
    {
        public PartyRelationshipEditComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public MatDatepicker<PartyRelationshipEditComponent> FromDate => this.MatDatepicker(roleType: M.PartyRelationship.FromDate);

        public MatDatepicker<PartyRelationshipEditComponent> ThroughDate => this.MatDatepicker(roleType: M.PartyRelationship.ThroughDate);

        public MatSelect<PartyRelationshipEditComponent> Employer => this.MatSelect(roleType: M.Employment.Employer);

        public MatSelect<PartyRelationshipEditComponent> Employee => this.MatSelect(roleType: M.Employment.Employee);

        public MatSelect<PartyRelationshipEditComponent> Contact => this.MatSelect(roleType: M.OrganisationContactRelationship.Contact);

        public MatSelect<PartyRelationshipEditComponent> Organisation => this.MatSelect(roleType: M.OrganisationContactRelationship.Organisation);

        public MatSelect<PartyRelationshipEditComponent> ContactKinds => this.MatSelect(roleType: M.OrganisationContactRelationship.ContactKinds);

        public Button<PartyRelationshipEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<PartyRelationshipEditComponent> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<PartyRelationshipEditComponent> NewpartyRelationship => this.Button(By.CssSelector("div[data-allors-class='partyrelationship']"));
    }
}
