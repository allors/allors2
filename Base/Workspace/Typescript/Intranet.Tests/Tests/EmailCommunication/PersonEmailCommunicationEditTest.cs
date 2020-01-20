// <copyright file="PersonEmailCommunicationEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.EmailCommunicationTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.emailcommunication.edit;
    using src.allors.material.@base.objects.person.list;
    using src.allors.material.@base.objects.person.overview;
    using Xunit;

    [Collection("Test collection")]
    public class PersonEmailCommunicationEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        public PersonEmailCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Edit()
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.DisplayName().Equals("John Doe"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First();

            var employeeEmailAddress = employee.PersonalEmailAddress;
            var personEmailAddress = person.PersonalEmailAddress;

            var editCommunicationEvent = new EmailCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromEmail(employeeEmailAddress)
                .WithFromParty(employee)
                .WithToParty(person)
                .WithToEmail(personEmailAddress)
                .WithEmailTemplate(new EmailTemplateBuilder(this.Session).Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var before = new EmailCommunications(this.Session).Extent().ToArray();

            this.personListPage.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.personListPage.Driver);

            var communicationEventOverview = personOverview.CommunicationeventOverviewPanel.Click();
            var row = communicationEventOverview.Table.FindRow(editCommunicationEvent);
            var cell = row.FindCell("description");
            cell.Click();

            var emailCommunicationEdit = new EmailCommunicationEditComponent(this.Driver);
            emailCommunicationEdit
                .CommunicationEventState.Set(new CommunicationEventStates(this.Session).Completed.Name)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry.Name)
                .FromParty.Set(person.DisplayName())
                .FromEmail.Set(personEmailAddress.ElectronicAddressString)
                .ToParty.Set(employee.DisplayName())
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
            Assert.Equal(person, editCommunicationEvent.FromParty);
            Assert.Equal(personEmailAddress, editCommunicationEvent.FromEmail);
            Assert.Equal(employee, editCommunicationEvent.ToParty);
            Assert.Equal(employeeEmailAddress, editCommunicationEvent.ToEmail);
            Assert.Equal("new subject", editCommunicationEvent.EmailTemplate.SubjectTemplate);
            Assert.Equal("new body", editCommunicationEvent.EmailTemplate.BodyTemplate);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ScheduledStart);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ScheduledEnd.Value.Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ActualStart.Value.Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ActualEnd.Value.Date);
        }
    }
}
