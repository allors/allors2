// <copyright file="WorkTaskTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Xunit;

    public class WorkTaskTests : DomainTest
    {
        [Fact]
        public void GivenWorkTask_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();

            Assert.Equal(new WorkEffortStates(this.Session).Created, workEffort.WorkEffortState);
            Assert.Equal(workEffort.LastWorkEffortState, workEffort.WorkEffortState);
        }

        [Fact]
        public void GivenWorkTask_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            this.Session.Derive();

            Assert.Null(workEffort.PreviousWorkEffortState);
        }

        [Fact]
        public void GivenWorkEffortAndTimeEntries_WhenDeriving_ThenActualHoursDerived()
        {
            // Arrange
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            this.Session.Derive(true);

            var yesterday = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(this.Session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var timeEntry1 = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(yesterday)
                .WithThroughDate(laterYesterday)
                .WithWorkEffort(workOrder)
                .Build();

            var timeEntry2 = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(today)
                .WithThroughDate(laterToday)
                .WithWorkEffort(workOrder)
                .Build();

            var timeEntry3 = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(tomorrow)
                .WithThroughDate(laterTomorrow)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry1);
            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry2);
            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntry3);

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.Equal(13.0M, workOrder.ActualHours);
        }

        [Fact]
        public void GivenWorkEffortAndTimeEntries_WhenDeriving_ThenActualStartAndCompletionDerived()
        {
            // Arrange
            var frequencies = new TimeFrequencies(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(internalOrganisation).Build();

            this.Session.Derive(true);

            var yesterday = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(this.Session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var timeEntryToday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(today)
                .WithThroughDate(laterToday)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday);

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.Equal(today, workOrder.ActualStart);
            Assert.Equal(laterToday, workOrder.ActualCompletion);

            //// Re-arrange
            var timeEntryYesterday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(yesterday)
                .WithThroughDate(laterYesterday)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday);

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.Equal(yesterday, workOrder.ActualStart);
            Assert.Equal(laterToday, workOrder.ActualCompletion);

            //// Re-arrange

            var timeEntryTomorrow = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(tomorrow)
                .WithThroughDate(laterTomorrow)
                .WithTimeFrequency(frequencies.Minute)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow);

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.Equal(yesterday, workOrder.ActualStart);
            Assert.Equal(laterTomorrow, workOrder.ActualCompletion);
        }

        [Fact]
        public void GivenWorkEffort_WhenDeriving_ThenPrintDocumentCreated()
        {
            // Arrange
            var frequencies = new TimeFrequencies(this.Session);
            var purposes = new ContactMechanismPurposes(this.Session);

            //// Customer Contact and Address Data
            var customer = new OrganisationBuilder(this.Session).WithName("Customer").Build();
            var customerContact = new PersonBuilder(this.Session).WithFirstName("Customer").WithLastName("Contact").Build();
            var organisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            var customerRelation = new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(organisation).Build();

            var usa = new Countries(this.Session).Extent().First(c => c.IsoCode.Equals("US"));
            var michigan = new StateBuilder(this.Session).WithName("Michigan").WithCountry(usa).Build();
            var northville = new CityBuilder(this.Session).WithName("Northville").WithState(michigan).Build();
            var postalCode = new PostalCodeBuilder(this.Session).WithCode("48167").Build();
            var billingAddress = this.CreatePostalAddress("Billing Address", "123 Street", "Suite S1", northville, postalCode);
            var shippingAddress = this.CreatePostalAddress("Shipping Address", "123 Street", "Dock D1", northville, postalCode);
            var phone = new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("1").WithAreaCode("616").WithContactNumber("774-2000").Build();

            customer.AddPartyContactMechanism(this.CreatePartyContactMechanism(purposes.BillingAddress, billingAddress));
            customer.AddPartyContactMechanism(this.CreatePartyContactMechanism(purposes.ShippingAddress, shippingAddress));
            customerContact.AddPartyContactMechanism(this.CreatePartyContactMechanism(purposes.GeneralPhoneNumber, phone));

            //// Work Effort Data
            var salesPerson = new PersonBuilder(this.Session).WithFirstName("Sales").WithLastName("Person").Build();
            var salesOrder = this.CreateSalesOrder(customer, organisation);
            var workOrder = this.CreateWorkEffort(organisation, customer, customerContact, salesOrder.SalesOrderItems.First);
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            var employment = new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(organisation).Build();

            var salesOrderItem = salesOrder.SalesOrderItems.First;
            ((SalesOrderDerivedRoles)salesOrder).AddValidOrderItem(salesOrderItem);

            //// Work Effort Inventory Assignmets
            var part1 = this.CreatePart("P1");
            var part2 = this.CreatePart("P2");
            var part3 = this.CreatePart("P3");

            this.Session.Derive(true);

            var inventoryAssignment1 = this.CreateInventoryAssignment(workOrder, part1, 11);
            var inventoryAssignment2 = this.CreateInventoryAssignment(workOrder, part2, 12);
            var inventoryAssignment3 = this.CreateInventoryAssignment(workOrder, part3, 13);

            //// Work Effort Time Entries
            var yesterday = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(this.Session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var timeEntryYesterday = this.CreateTimeEntry(yesterday, laterYesterday, frequencies.Day, workOrder);
            var timeEntryToday = this.CreateTimeEntry(today, laterToday, frequencies.Hour, workOrder);
            var timeEntryTomorrow = this.CreateTimeEntry(tomorrow, laterTomorrow, frequencies.Minute, workOrder);

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday);
            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday);
            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow);

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.True(workOrder.ExistPrintDocument);
        }

        [Fact]
        public void GivenWorkEffortPrintDocument_WhenPrinting_ThenMediaCreated()
        {
            // Arrange
            var frequencies = new TimeFrequencies(this.Session);
            var purposes = new ContactMechanismPurposes(this.Session);

            //// Customer Contact and Address Data
            var customer = new OrganisationBuilder(this.Session).WithName("Customer").Build();
            var customerContact = new PersonBuilder(this.Session).WithFirstName("Customer").WithLastName("Contact").Build();
            var organisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            var customerRelation = new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(organisation).Build();

            var usa = new Countries(this.Session).Extent().First(c => c.IsoCode.Equals("US"));
            var michigan = new StateBuilder(this.Session).WithName("Michigan").WithCountry(usa).Build();
            var northville = new CityBuilder(this.Session).WithName("Northville").WithState(michigan).Build();
            var postalCode = new PostalCodeBuilder(this.Session).WithCode("48167").Build();
            var billingAddress = this.CreatePostalAddress("Billing Address", "123 Street", "Suite S1", northville, postalCode);
            var shippingAddress = this.CreatePostalAddress("Shipping Address", "123 Street", "Dock D1", northville, postalCode);
            var phone = new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("1").WithAreaCode("616").WithContactNumber("774-2000").Build();

            customer.AddPartyContactMechanism(this.CreatePartyContactMechanism(purposes.BillingAddress, billingAddress));
            customer.AddPartyContactMechanism(this.CreatePartyContactMechanism(purposes.ShippingAddress, shippingAddress));
            customerContact.AddPartyContactMechanism(this.CreatePartyContactMechanism(purposes.GeneralPhoneNumber, phone));

            //// Work Effort Data
            var salesPerson = new PersonBuilder(this.Session).WithFirstName("Sales").WithLastName("Person").Build();
            var salesOrder = this.CreateSalesOrder(customer, organisation);
            var workOrder = this.CreateWorkEffort(organisation, customer, customerContact, salesOrder.SalesOrderItems.First);
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            var employment = new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(organisation).Build();

            this.Session.Derive(true);

            var salesOrderItem = salesOrder.SalesOrderItems.First;
            ((SalesOrderDerivedRoles)salesOrder).AddValidOrderItem(salesOrderItem);

            //// Work Effort Inventory Assignmets
            var part1 = this.CreatePart("P1");
            var part2 = this.CreatePart("P2");
            var part3 = this.CreatePart("P3");

            this.Session.Derive(true);

            var inventoryAssignment1 = this.CreateInventoryAssignment(workOrder, part1, 11);
            var inventoryAssignment2 = this.CreateInventoryAssignment(workOrder, part2, 12);
            var inventoryAssignment3 = this.CreateInventoryAssignment(workOrder, part3, 13);

            this.Session.Derive(true);

            //// Work Effort Time Entries
            var yesterday = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(this.Session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var timeEntryYesterday = this.CreateTimeEntry(yesterday, laterYesterday, frequencies.Day, workOrder);
            var timeEntryToday = this.CreateTimeEntry(today, laterToday, frequencies.Hour, workOrder);
            var timeEntryTomorrow = this.CreateTimeEntry(tomorrow, laterTomorrow, frequencies.Minute, workOrder);

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday);
            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday);
            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow);

            this.Session.Derive(true);

            // Act
            workOrder.Print();

            this.Session.Derive();
            this.Session.Commit();

            // Assert
            Assert.True(workOrder.PrintDocument.ExistMedia);

            var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var outputFile = System.IO.File.Create(System.IO.Path.Combine(desktopDir, "workTask.odt"));
            var stream = new System.IO.MemoryStream(workOrder.PrintDocument.Media.MediaContent.Data);

            stream.CopyTo(outputFile);
            stream.Close();
        }

        [Fact]
        public void GivenWorkEffortAndTimeEntriesWithBillingRate_WhenInvoiced_ThenTimeEntryBillingRateIsUsed()
        {
            var frequencies = new TimeFrequencies(this.Session);

            var organisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            var customer = new PersonBuilder(this.Session).WithLastName("Customer").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(organisation).Build();
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(organisation).Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).Build();

            this.Session.Derive(true);

            var yesterday = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(this.Session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var timeEntryYesterday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(yesterday)
                .WithThroughDate(laterYesterday)
                .WithWorkEffort(workOrder)
                .WithAssignedBillingRate(10)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday);

            var timeEntryToday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(today)
                .WithThroughDate(laterToday)
                .WithWorkEffort(workOrder)
                .WithAssignedBillingRate(12)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday);

            var timeEntryTomorrow = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(tomorrow)
                .WithThroughDate(laterTomorrow)
                .WithWorkEffort(workOrder)
                .WithAssignedBillingRate(12)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow);

            workOrder.Complete();

            this.Session.Derive(true);

            workOrder.Invoice();

            var salesInvoice = customer.SalesInvoicesWhereBillToCustomer.First;

            Assert.Equal(2, salesInvoice.InvoiceItems.Length);
            Assert.Equal(10, timeEntryYesterday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.AssignedUnitPrice);
            Assert.Equal(3, timeEntryYesterday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.Quantity);
            Assert.Equal(12, timeEntryToday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.AssignedUnitPrice);
            Assert.Equal(10, timeEntryToday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.Quantity);
        }

        [Fact]
        public void GivenParentWorkEffortAndTimeEntriesWithBillingRate_WhenInvoiced_ThenTimeEntryBillingRateIsUsed()
        {
            var frequencies = new TimeFrequencies(this.Session);

            var organisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").WithPartyContactMechanism(billToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(organisation).Build();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(organisation).Build();

            var parentWorkOrder = new WorkTaskBuilder(this.Session).WithName("Parent Task").WithCustomer(customer).Build();

            this.Session.Derive(true);

            var yesterday = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(this.Session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var timeEntryYesterday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(yesterday)
                .WithThroughDate(laterYesterday)
                .WithWorkEffort(parentWorkOrder)
                .WithAssignedBillingRate(10)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday);

            var timeEntryToday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(today)
                .WithThroughDate(laterToday)
                .WithWorkEffort(parentWorkOrder)
                .WithAssignedBillingRate(12)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday);

            var childWorkOrder = new WorkTaskBuilder(this.Session).WithName("Child Task").WithCustomer(customer).Build();
            parentWorkOrder.AddChild(childWorkOrder);

            var timeEntryTomorrow = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(tomorrow)
                .WithThroughDate(laterTomorrow)
                .WithWorkEffort(childWorkOrder)
                .WithAssignedBillingRate(14)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow);

            childWorkOrder.Complete();

            this.Session.Derive(true);

            parentWorkOrder.Complete();

            this.Session.Derive(true);

            parentWorkOrder.Invoice();

            this.Session.Derive(true);

            var salesInvoice = customer.SalesInvoicesWhereBillToCustomer.First;

            Assert.Equal(3, salesInvoice.InvoiceItems.Length);
            Assert.Equal(10, timeEntryYesterday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.AssignedUnitPrice);
            Assert.Equal(3, timeEntryYesterday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.Quantity);
            Assert.Equal(12, timeEntryToday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.AssignedUnitPrice);
            Assert.Equal(4, timeEntryToday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.Quantity);
            Assert.Equal(14, timeEntryTomorrow.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.AssignedUnitPrice);
            Assert.Equal(6, timeEntryTomorrow.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.Quantity);
        }

        [Fact]
        public void GivenWorkEffortAndTimeEntriesWithoutBillingRate_WhenInvoiced_ThenWorkEffortRateIsUsed()
        {
            var frequencies = new TimeFrequencies(this.Session);

            var organisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            var customer = new PersonBuilder(this.Session).WithLastName("Customer").Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(organisation).Build();
            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(organisation).Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).Build();

            new WorkEffortAssignmentRateBuilder(this.Session).WithWorkEffort(workOrder).WithRate(10).WithRateType(new RateTypes(this.Session).StandardRate).Build();

            this.Session.Derive(true);

            var yesterday = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(this.Session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var timeEntryYesterday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(yesterday)
                .WithThroughDate(laterYesterday)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday);

            var timeEntryToday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(today)
                .WithThroughDate(laterToday)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday);

            var timeEntryTomorrow = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(tomorrow)
                .WithThroughDate(laterTomorrow)
                .WithWorkEffort(workOrder)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow);

            workOrder.Complete();

            this.Session.Derive(true);

            workOrder.Invoice();

            var salesInvoice = customer.SalesInvoicesWhereBillToCustomer.First;

            Assert.Single(salesInvoice.InvoiceItems);
            Assert.Equal(10, timeEntryYesterday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.AssignedUnitPrice);
            Assert.Equal(13, timeEntryYesterday.TimeEntryBillingsWhereTimeEntry.First.InvoiceItem.Quantity);
        }

        [Fact]
        public void GivenWorkEffortAndPartsUsed_WhenInvoiced_ThenPartsAreInvoiced()
        {
            var organisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);

            var customerEmail = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new EmailAddressBuilder(this.Session).WithElectronicAddressString($"customer@acme.com").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("Customer").WithPartyContactMechanism(customerEmail).Build();
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(organisation).Build();

            var employee = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            new EmploymentBuilder(this.Session).WithEmployee(employee).WithEmployer(organisation).Build();

            var yesterday = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(-1));

            var today = DateTimeFactory.CreateDateTime(this.Session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.Session.Now().AddDays(1));

            var part1 = this.CreatePart("P1");

            this.Session.Derive(true);

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part1)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(11)
                .Build();

            this.Session.Derive(true);

            var part1BasePriceYesterday = new BasePriceBuilder(this.Session)
                .WithDescription("baseprice part1")
                .WithPrice(9)
                .WithPart(part1)
                .WithFromDate(yesterday)
                .WithThroughDate(today)
                .Build();

            var part1BasePriceToday = new BasePriceBuilder(this.Session)
                .WithDescription("baseprice part1")
                .WithPrice(10)
                .WithPart(part1)
                .WithFromDate(today)
                .WithThroughDate(tomorrow)
                .Build();

            var part1BasePriceTomorrow = new BasePriceBuilder(this.Session)
                .WithDescription("baseprice part1")
                .WithPrice(11)
                .WithPart(part1)
                .WithFromDate(tomorrow)
                .Build();

            var workOrder = new WorkTaskBuilder(this.Session).WithName("Task").WithCustomer(customer).Build();

            this.Session.Derive(true);

            var timeEntryToday = new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(today)
                .WithThroughDate(laterToday)
                .WithWorkEffort(workOrder)
                .WithAssignedBillingRate(12)
                .Build();

            employee.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday);

            new WorkEffortInventoryAssignmentBuilder(this.Session).WithAssignment(workOrder).WithInventoryItem(part1.InventoryItemsWherePart.First).WithQuantity(3).Build();

            this.Session.Derive(true);

            workOrder.Complete();

            this.Session.Derive(true);

            workOrder.Invoice();

            this.Session.Derive(true);

            var salesInvoice = customer.SalesInvoicesWhereBillToCustomer.First;

            Assert.Equal(2, salesInvoice.InvoiceItems.Length);
            Assert.Equal(10, workOrder.WorkEffortBillingsWhereWorkEffort.First.InvoiceItem.AssignedUnitPrice);
            Assert.Equal(30, workOrder.WorkEffortBillingsWhereWorkEffort.First.InvoiceItem.TotalExVat);
        }

        private Part CreatePart(string id) =>
            new NonUnifiedPartBuilder(this.Session)
            .WithProductIdentification(new PartNumberBuilder(this.Session)
            .WithIdentification(id)
            .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
            .Build();

        private WorkEffortInventoryAssignment CreateInventoryAssignment(WorkEffort workOrder, Part part, int quantity)
        {
            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(quantity)
                .Build();

            this.Session.Derive();

            return new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workOrder)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(quantity)
                .Build();
        }

        private TimeEntry CreateTimeEntry(DateTime fromDate, DateTime throughDate, TimeFrequency frequency, WorkEffort workEffort) =>
            new TimeEntryBuilder(this.Session)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFromDate(fromDate)
                .WithThroughDate(throughDate)
                .WithTimeFrequency(frequency)
                .WithWorkEffort(workEffort)
                .Build();

        private PostalAddress CreatePostalAddress(string addressLine1,
            string addressLine2,
            string addressLine3,
            City city,
            PostalCode postalCode) =>
                new PostalAddressBuilder(this.Session)
                .WithAddress1(addressLine1)
                .WithAddress2(addressLine2)
                .WithAddress3(addressLine3)
                .WithPostalAddressBoundary(postalCode)
                .WithPostalAddressBoundary(city)
                .WithPostalAddressBoundary(city.State.Country)
                .Build();

        private PartyContactMechanism CreatePartyContactMechanism(ContactMechanismPurpose purpose, ContactMechanism mechanism) =>
            new PartyContactMechanismBuilder(this.Session)
            .WithContactPurpose(purpose)
            .WithContactMechanism(mechanism)
            .WithUseAsDefault(true)
            .Build();

        private SalesOrder CreateSalesOrder(Party customer, InternalOrganisation takenBy) =>
            new SalesOrderBuilder(this.Session)
            .WithShipToCustomer(customer)
            .WithTakenBy(takenBy)
            .WithSalesOrderItem(new SalesOrderItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Service)
                .WithAssignedUnitPrice(1)
                .Build())
            .WithSalesTerm(new OrderTermBuilder(this.Session)
                .WithDescription("Net 30")
                .WithTermType(new InvoiceTermTypes(this.Session).PaymentNetDays)
                .Build())
            .Build();

        private WorkEffort CreateWorkEffort(Organisation takenBy, Party customer, Person contact, SalesOrderItem salesOrderItem) =>
            new WorkTaskBuilder(this.Session)
            .WithName("Task")
            .WithTakenBy(takenBy)
            .WithFacility(new Facilities(this.Session).Extent().First)
            .WithCustomer(customer)
            .WithContactPerson(contact)
            .WithWorkEffortPurpose(new WorkEffortPurposes(this.Session).Maintenance)
            .WithOrderItemFulfillment(salesOrderItem)
            .WithSpecialTerms("Net 45 Days")
            .Build();
    }
}
