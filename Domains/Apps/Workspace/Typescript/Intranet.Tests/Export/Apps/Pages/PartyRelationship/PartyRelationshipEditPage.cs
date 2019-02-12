namespace Pages.PartyRelationshipTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PartyRelationshipEditPage : MainPage
    {
        public PartyRelationshipEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialDatePicker<PartyRelationshipEditPage> FromDate => this.MaterialDatePicker(roleType: M.PartyRelationship.FromDate);

        public MaterialDatePicker<PartyRelationshipEditPage> ThroughDate => this.MaterialDatePicker(roleType: M.PartyRelationship.ThroughDate);

        public MaterialSingleSelect<PartyRelationshipEditPage> Employer => this.MaterialSingleSelect(roleType: M.Employment.Employer);

        public MaterialSingleSelect<PartyRelationshipEditPage> Employee => this.MaterialSingleSelect(roleType: M.Employment.Employee);

        public MaterialSingleSelect<PartyRelationshipEditPage> Contact => this.MaterialSingleSelect(roleType: M.OrganisationContactRelationship.Contact);

        public MaterialSingleSelect<PartyRelationshipEditPage> Organisation => this.MaterialSingleSelect(roleType: M.OrganisationContactRelationship.Organisation);

        public MaterialMultipleSelect<PartyRelationshipEditPage> ContactKinds => this.MaterialMultipleSelect(roleType: M.OrganisationContactRelationship.ContactKinds);

        public Button<PartyRelationshipEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<PartyRelationshipEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<PartyRelationshipEditPage> NewpartyRelationship => this.Button(By.CssSelector("div[data-allors-class='partyrelationship']"));
    }
}
