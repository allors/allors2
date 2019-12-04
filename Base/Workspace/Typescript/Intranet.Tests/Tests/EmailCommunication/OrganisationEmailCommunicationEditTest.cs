// <copyright file="OrganisationEmailCommunicationEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.EmailCommunicationTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.emailcommunication.edit;
    using src.allors.material.@base.objects.organisation.list;
    using src.allors.material.@base.objects.organisation.overview;
    using Xunit;

    [Collection("Test collection")]
    public class OrganisationEmailCommunicationEditTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public OrganisationEmailCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Edit()
        {
            var organisations = new Organisations(this.Session).Extent();
            var organisation = organisations.First(v => v.PartyName.Equals("Acme"));
            var contact = organisation.CurrentContacts.First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            var employeeEmailAddress = employee.PersonalEmailAddress;
            var personEmailAddress = organisation.CurrentContacts.First.PersonalEmailAddress;

            var editCommunicationEvent = new EmailCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromParty(employee)
                .WithFromEmail(employeeEmailAddress)
                .WithToParty(organisation.CurrentContacts.First)
                .WithToEmail(personEmailAddress)
                .WithEmailTemplate(new EmailTemplateBuilder(this.Session).Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var before = new EmailCommunications(this.Session).Extent().ToArray();

            this.organisationListPage.Table.DefaultAction(organisation);
            var organisationOverview = new OrganisationOverviewComponent(this.organisationListPage.Driver);

            var communicationEventOverview = organisationOverview.CommunicationeventOverviewPanel.Click();
            var row = communicationEventOverview.Table.FindRow(editCommunicationEvent);
            var cell = row.FindCell("description");
            cell.Click();

            var emailCommunicationEdit = new EmailCommunicationEditComponent(organisationOverview.Driver);
            emailCommunicationEdit
                .CommunicationEventState.Set(new CommunicationEventStates(this.Session).Completed.Name)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry.Name)
                .FromParty.Set(contact.PartyName)
                .FromEmail.Set(personEmailAddress.ElectronicAddressString)
                .ToParty.Set(employee.PartyName)
                .ToEmail.Set(employeeEmailAddress.ElectronicAddressString)
                .SubjectTemplate.Set("new subject")
                .BodyTemplate.Set("new body")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Inquiry, editCommunicationEvent.EventPurposes);
            Assert.Equal(contact, editCommunicationEvent.FromParty);
            Assert.Equal(personEmailAddress, editCommunicationEvent.FromEmail);
            Assert.Equal(employee, editCommunicationEvent.ToParty);
            Assert.Equal(employeeEmailAddress, editCommunicationEvent.ToEmail);
            Assert.Equal("new subject", editCommunicationEvent.EmailTemplate.SubjectTemplate);
            Assert.Equal("new body", editCommunicationEvent.EmailTemplate.BodyTemplate);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledStart);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledEnd.Value.Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date);
        }
    }
}
