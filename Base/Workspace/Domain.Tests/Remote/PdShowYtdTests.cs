namespace Desktop.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using NUnit.Framework;
    using Pages;
    using Populations;
    using Should;

    public class PdShowYtdTests : Test
    {
        private Population population;

        public Pillar Pillar { get; set; }


        public PillarDefinitionPage Page { get; private set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            this.population = new Population(this.Session);

            this.population.Guarulhos.AddM2SAdmin(this.population.BenWegner);

            this.Session.Derive(true);
            this.Session.Commit();

            this.Pillar = this.population.PillarProduction2015;
            
            this.Driver.Navigate().GoToUrl(Test.AppUrl);
            this.Page = new PillarDefinitionPage(this.Driver);

            this.Page.GoToPillarDefinition();
        }

        [Test]
        public void Navigate()
        {
            this.Page.IsPillarDefinitionPage.ShouldBeTrue();
        }

        [Test]
        public void TestCountStandardsElementsAndRequirements()
        {
            this.Page.Filter(this.Pillar);

            this.Page.Standards.Count().ShouldEqual(3);

            this.Page.SelectStandard(this.population.StandardWork);

            this.Page.Elements.Count().ShouldEqual(2);

            this.Page.SelectElement(this.population.ElementStandardWorkInputs);
            
            this.Page.Requirements.Count().ShouldEqual(5);
        }

        [Test]
        public void TestUpdateAndSavePillar()
        {
            this.Page.Filter(this.Pillar);

            var yearField = this.Page.PillarYearEditField;
            yearField.Clear();
            yearField.SendKeys("2016");

            this.Page.Save();

            this.Page.WaitForToaster();

            // Always rollback your session
            this.Session.Rollback();

            // Now you can check your database
            this.Pillar.Year.ShouldEqual(2016);

        }
    }
}