using src.allors.material.apps.objects.emailcommunication.edit;
using src.allors.material.apps.objects.facetofacecommunication.edit;
using src.allors.material.apps.objects.lettercorrespondence.edit;
using src.allors.material.apps.objects.organisation.create;
using src.allors.material.apps.objects.phonecommunication.edit;

namespace src.allors.material.apps.objects.organisation.overview
{
    using Components;

    using OpenQA.Selenium;

    public partial class OrganisationOverviewComponent
    {
        public Button<OrganisationOverviewComponent> BtnFaceToFaceCommunication => this.Button(By.CssSelector("button[data-allors-class='FaceToFaceCommunication']"));

        public Button<OrganisationOverviewComponent> BtnEmailCommunication => this.Button(By.CssSelector("button[data-allors-class='EmailCommunication']"));

        public Button<OrganisationOverviewComponent> BtnLetterCorrespondence => this.Button(By.CssSelector("button[data-allors-class='LetterCorrespondence']"));

        public Button<OrganisationOverviewComponent> BtnPhoneCommunication => this.Button(By.CssSelector("button[data-allors-class='PhoneCommunication']"));

        public Button<OrganisationOverviewComponent> BtnCustomerRelationship => this.Button(By.CssSelector("button[data-allors-class='CustomerRelationship']"));

        public Button<OrganisationOverviewComponent> BtnEmployment => this.Button(By.CssSelector("button[data-allors-class='Employment']"));

        public Button<OrganisationOverviewComponent> BtnSupplierRelationship => this.Button(By.CssSelector("button[data-allors-class='SupplierRelationship']"));

        public Button<OrganisationOverviewComponent> BtnOrganisationContactRelationship => this.Button(By.CssSelector("button[data-allors-class='OrganisationContactRelationship']"));
    }
}
