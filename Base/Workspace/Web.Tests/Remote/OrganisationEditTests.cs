using Allors.Domain;

namespace Tests.Remote
{
    using Allors;

    using NUnit.Framework;

    using Should;

    using Tests.Remote.Pages;

    public class OrganisationEditTests : Test
    {
        private Population population;

        private Person johnDoe;
        private Person janeDoe;

        public OrganisationEditPage Page { get; private set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            this.population = new Population(this.Session);

            this.johnDoe = new PersonBuilder(this.Session)
                .WithUserName("johndoe")
                .WithFirstName("John")
                .WithLastName("Doe")
                .Build();

            this.janeDoe = new PersonBuilder(this.Session)
                .WithUserName("janedoe")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            this.Session.Derive(true);
            this.Session.Commit();
            
            this.Driver.Navigate().GoToUrl(Test.AppUrl);
            this.Page = new OrganisationEditPage(this.Driver);
        }

        [Test]
        public void Navigate()
        {
            this.Page.GoToOrganisations();
            this.Page.IsOrganisationPage.ShouldBeTrue();
        }

        [Test]
        public void EditOrganisation()
        {
            var organisation = new OrganisationBuilder(this.Session)
                .WithName("Acme")
                .Build();

            this.Session.Derive(true);
            this.Session.Commit();

            this.Page
                .GoToOrganisations()
                .Edit(organisation)
                .EnterName("Acme 2")
                .AddEmployees(this.johnDoe, this.janeDoe)
                .Save()
                .SaveSuccessful.ShouldBeTrue();

            this.Session.Rollback();

            organisation.Name.ShouldEqual("Acme 2");
        }
    }
}