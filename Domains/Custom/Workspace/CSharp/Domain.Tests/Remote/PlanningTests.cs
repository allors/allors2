namespace Desktop.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Desktop.Tests.Pages;
    using Desktop.Tests.Populations;

    using NUnit.Framework;

    using Should;

    public class PlanningTests : Test
    {
        private Population population;

        public PillarAssessment PillarAssessment { get; set; }

        public IEnumerable<RequirementAssessment> RequirementAssessments
        {
            get
            {
                return this.PillarAssessment.StandardAssessments.SelectMany(x => x.ElementAssessments.SelectMany(y => y.RequirementAssessments));
            }
        }

        public ExecutionPage Page { get; private set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            this.population = new Population(this.Session);

            this.population.Guarulhos.AddM2SAdmin(this.population.BenWegner);

            this.Session.Derive(true);
            this.Session.Commit();

            var assessmentYear = new AssessmentYearBuilder(this.Session)
                .WithYear(2015)
                .WithPillarConfiguration(this.population.PillarProduction2015.PlantNonUnionConfiguration)
                .WithReportingUnit(this.population.Guarulhos)
                .Build();

            this.Session.Derive(true);
            this.Session.Commit();

            this.PillarAssessment = assessmentYear.Assessment.PillarAssessments[0];

            foreach (var requirementAssessment in this.RequirementAssessments)
            {
                if (requirementAssessment.Requirement.Equals(this.population.RequirementTaktTime)
                    || requirementAssessment.Requirement.Equals(this.population.RequirementProcessLeadersUnderstandCalculation))
                {
                    requirementAssessment.InitialScore = Score.LimitedEvidenceOfUse;
                    requirementAssessment.GapComments = "Push it forward ...";
                    requirementAssessment.CompleteAssessment();

                    this.Session.Derive(true);
                    this.Session.Commit();
                    
                    requirementAssessment.TargetScore = Score.ExceedsRequirements;
                    requirementAssessment.TargetDate = DateTime.UtcNow.AddDays(1);
                    requirementAssessment.Responsible = this.population.BenWegner;
                    requirementAssessment.CompletePlanning();

                    this.Session.Derive(true);
                    this.Session.Commit();

                    requirementAssessment.ApprovePlanning();

                    this.Session.Derive(true);
                    this.Session.Commit();
                }
            }


            this.Driver.Navigate().GoToUrl(Test.AppUrl);

            this.Page = new ExecutionPage(this.Driver);
            this.Page.GoToExecution();
        }
    }
}