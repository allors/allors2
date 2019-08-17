// <copyright file="CommunicationEventListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.CommunicationEventTests
{
    using Allors.Domain;
    using Allors.Meta;
    using src.allors.material.@base.objects.communicationevent.list;
    using Xunit;

    [Collection("Test collection")]
    public class CommunicationEventListTest : Test
    {
        private readonly CommunicationEventListComponent page;

        public CommunicationEventListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToCommunicationEvents();
        }

        [Fact]
        public void Title() => Assert.Equal("Communications", this.Driver.Title);

        [Fact]
        public void Table()
        {
            var communicationEvent = new CommunicationEvents(this.Session).FindBy(M.CommunicationEvent.Subject, "meeting");

            var cell = this.page.Table
                .FindRow(communicationEvent)
                .FindCell("subject");

            Assert.Equal("meeting", cell.Element.Text);
        }

        [Fact]
        public void Create()
        {
            var dialog = this.page.CreateEmailCommunication();
            dialog.CANCEL.Click();
        }
    }
}
