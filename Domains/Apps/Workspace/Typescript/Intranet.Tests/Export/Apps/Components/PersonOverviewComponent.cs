namespace src.allors.material.apps.objects.person.overview
{
    using Components;

    using OpenQA.Selenium;

    public partial class PersonOverviewComponent 
    {
        public Anchor<PersonOverviewComponent> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public Button<PersonOverviewComponent> BtnFaceToFaceCommunication => this.Button(By.CssSelector("button[data-allors-class='FaceToFaceCommunication']"));

        public Button<PersonOverviewComponent> BtnEmailCommunication => this.Button(By.CssSelector("button[data-allors-class='EmailCommunication']"));

        public Button<PersonOverviewComponent> BtnLetterCorrespondence => this.Button(By.CssSelector("button[data-allors-class='LetterCorrespondence']"));

        public Button<PersonOverviewComponent> BtnPhoneCommunication => this.Button(By.CssSelector("button[data-allors-class='PhoneCommunication']"));

        public Button<PersonOverviewComponent> BtnPostalAddress => this.Button(By.CssSelector("button[data-allors-class='PostalAddress']"));

        public Button<PersonOverviewComponent> BtnTelecommunicationsNumber => this.Button(By.CssSelector("button[data-allors-class='TelecommunicationsNumber']"));

        public Button<PersonOverviewComponent> BtnEmailAddress => this.Button(By.CssSelector("button[data-allors-class='EmailAddress']"));

        public Button<PersonOverviewComponent> BtnWebAddress => this.Button(By.CssSelector("button[data-allors-class='WebAddress']"));

        public Button<PersonOverviewComponent> BtnCustomerRelationship => this.Button(By.CssSelector("button[data-allors-class='CustomerRelationship']"));

        public Button<PersonOverviewComponent> BtnEmployment => this.Button(By.CssSelector("button[data-allors-class='Employment']"));

        public Button<PersonOverviewComponent> BtnOrganisationContactRelationship => this.Button(By.CssSelector("button[data-allors-class='OrganisationContactRelationship']"));
    }
}
