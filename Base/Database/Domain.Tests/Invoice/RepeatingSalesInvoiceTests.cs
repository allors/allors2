// <copyright file="RepeatingSalesInvoiceTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    using Allors.Meta;

    using Xunit;

    public class RepeatingSalesInvoiceTests : DomainTest
    {
        [Fact]
        public void GivenRepeatingSalesInvoice_WhenDeriving_ThenFrequencyIsEitherMontlyOrWeekly()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithLocality("Muizen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var repeatingInvoice = new RepeatingSalesInvoiceBuilder(this.Session)
                .WithSource(invoice)
                .WithFrequency(new TimeFrequencies(this.Session).Month)
                .WithNextExecutionDate(this.Session.Now().AddDays(1))
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            var weekDay = repeatingInvoice.NextExecutionDate.DayOfWeek.ToString();
            var daysOfWeek = new DaysOfWeek(this.Session).Extent();
            daysOfWeek.Filter.AddEquals(M.Enumeration.Name, weekDay);
            var dayOfWeek = daysOfWeek.First;

            repeatingInvoice.Frequency = new TimeFrequencies(this.Session).Week;
            repeatingInvoice.DayOfWeek = dayOfWeek;

            Assert.False(this.Session.Derive(false).HasErrors);

            repeatingInvoice.Frequency = new TimeFrequencies(this.Session).Day;

            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);
            Assert.Contains(validation.Errors, v => v.Message.Contains("Selected frequency is not supported"));
        }

        [Fact]
        public void GivenWeeklyRepeatingSalesInvoice_WhenDeriving_ThenDayOfWeekMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithLocality("Muizen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var repeatingInvoice = new RepeatingSalesInvoiceBuilder(this.Session)
                .WithSource(invoice)
                .WithFrequency(new TimeFrequencies(this.Session).Week)
                .WithNextExecutionDate(this.Session.Now().AddDays(1))
                .Build();

            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);
            Assert.Contains(MetaRepeatingSalesInvoice.Instance.DayOfWeek, validation.Errors.First().RoleTypes);

            Assert.False(this.Session.Derive(false).HasErrors);

            var weekDay = repeatingInvoice.NextExecutionDate.DayOfWeek.ToString();
            var daysOfWeek = new DaysOfWeek(this.Session).Extent();
            daysOfWeek.Filter.AddEquals(M.Enumeration.Name, weekDay);
            var dayOfWeek = daysOfWeek.First;

            repeatingInvoice.DayOfWeek = dayOfWeek;

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenMontlyRepeatingSalesInvoice_WhenDeriving_ThenDayOfWeekMustNotExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithLocality("Muizen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var nextExecutionDate = this.Session.Now().AddDays(1);
            var weekDay = nextExecutionDate.DayOfWeek.ToString();
            var daysOfWeek = new DaysOfWeek(this.Session).Extent();
            daysOfWeek.Filter.AddEquals(M.Enumeration.Name, weekDay);
            var dayOfWeek = daysOfWeek.First;

            var repeatingInvoice = new RepeatingSalesInvoiceBuilder(this.Session)
                .WithSource(invoice)
                .WithFrequency(new TimeFrequencies(this.Session).Month)
                .WithDayOfWeek(dayOfWeek)
                .WithNextExecutionDate(nextExecutionDate)
                .Build();

            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);
            Assert.Contains(MetaRepeatingSalesInvoice.Instance.DayOfWeek, validation.Errors.First().RoleTypes);

            repeatingInvoice.RemoveDayOfWeek();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenWeeklyRepeatingSalesInvoice_WhenDeriving_ThenDayOfWeekMustMatchNextExecutionDate()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithLocality("Muizen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var nextExecutionDate = this.Session.Now().AddDays(1);
            var weekDay = nextExecutionDate.DayOfWeek.ToString();
            var daysOfWeek = new DaysOfWeek(this.Session).Extent();
            daysOfWeek.Filter.AddEquals(M.Enumeration.Name, weekDay);
            var dayOfWeek = daysOfWeek.First;

            var repeatingInvoice = new RepeatingSalesInvoiceBuilder(this.Session)
                .WithSource(invoice)
                .WithFrequency(new TimeFrequencies(this.Session).Week)
                .WithDayOfWeek(dayOfWeek)
                .WithNextExecutionDate(nextExecutionDate)
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            repeatingInvoice.NextExecutionDate = repeatingInvoice.NextExecutionDate.AddDays(1);
            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);
            Assert.Contains(MetaRepeatingSalesInvoice.Instance.DayOfWeek, validation.Errors.First().RoleTypes);
        }

        [Fact]
        public void GivenRepeatingSalesInvoice_WhenNotOnNextExecutionDate_ThenNothingChanges()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithLocality("Muizen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var mayThirteen2018 = new DateTime(2018, 5, 13, 12, 0, 0, DateTimeKind.Utc);
            var timeShift = mayThirteen2018 - this.Session.Now();
            this.TimeShift = timeShift;

            var countBefore = new SalesInvoices(this.Session).Extent().Count;

            var repeatingInvoice = new RepeatingSalesInvoiceBuilder(this.Session)
                .WithSource(invoice)
                .WithFrequency(new TimeFrequencies(this.Session).Week)
                .WithDayOfWeek(new DaysOfWeek(this.Session).Monday)
                .WithNextExecutionDate(mayThirteen2018.AddDays(1))
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            RepeatingSalesInvoices.Daily(this.Session);

            Assert.Equal(countBefore, new SalesInvoices(this.Session).Extent().Count);
            Assert.Equal(new DateTime(2018, 5, 14), repeatingInvoice.NextExecutionDate.Date);
            Assert.Empty(repeatingInvoice.SalesInvoices);
        }

        [Fact]
        public void GivenWeeklyRepeatingSalesInvoice_WhenDayArrives_ThenNextInvoiceIsCreated()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithLocality("Muizen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var mayFourteen2018 = new DateTime(2018, 5, 14, 12, 0, 0, DateTimeKind.Utc);
            var timeShift = mayFourteen2018 - this.Session.Now();
            this.TimeShift = timeShift;

            var countBefore = new SalesInvoices(this.Session).Extent().Count;

            var repeatingInvoice = new RepeatingSalesInvoiceBuilder(this.Session)
                .WithSource(invoice)
                .WithFrequency(new TimeFrequencies(this.Session).Week)
                .WithDayOfWeek(new DaysOfWeek(this.Session).Monday)
                .WithNextExecutionDate(mayFourteen2018)
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            RepeatingSalesInvoices.Daily(this.Session);

            Assert.Equal(countBefore + 1, new SalesInvoices(this.Session).Extent().Count);
            Assert.Equal(new DateTime(2018, 5, 21), repeatingInvoice.NextExecutionDate.Date);
            Assert.Single(repeatingInvoice.SalesInvoices);
        }

        [Fact]
        public void GivenMontlyRepeatingSalesInvoice_WhenDayArrives_ThenNextInvoiceIsCreated()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithLocality("Muizen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var mayFourteen2018 = new DateTime(2018, 5, 14, 12, 0, 0, DateTimeKind.Utc);
            var timeShift = mayFourteen2018 - this.Session.Now();
            this.TimeShift = timeShift;

            var countBefore = new SalesInvoices(this.Session).Extent().Count;

            var repeatingInvoice = new RepeatingSalesInvoiceBuilder(this.Session)
                .WithSource(invoice)
                .WithFrequency(new TimeFrequencies(this.Session).Month)
                .WithNextExecutionDate(mayFourteen2018)
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            RepeatingSalesInvoices.Daily(this.Session);

            Assert.Equal(countBefore + 1, new SalesInvoices(this.Session).Extent().Count);
            Assert.Equal(new DateTime(2018, 6, 14), repeatingInvoice.NextExecutionDate.Date);
            Assert.Single(repeatingInvoice.SalesInvoices);
        }

        [Fact]
        public void GivenWeeklyRepeatingSalesInvoice_WhenRepeating_ThenRepeatStopReachingLastExecutionDate()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithLocality("Muizen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            var mayFourteen2018 = new DateTime(2018, 5, 14, 12, 0, 0, DateTimeKind.Utc);
            var timeShift = mayFourteen2018 - DateTime.UtcNow;
            this.TimeShift = timeShift;

            new CustomerRelationshipBuilder(this.Session).WithFromDate(mayFourteen2018).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var countBefore = new SalesInvoices(this.Session).Extent().Count;

            var repeatingInvoice = new RepeatingSalesInvoiceBuilder(this.Session)
                .WithSource(invoice)
                .WithFrequency(new TimeFrequencies(this.Session).Week)
                .WithDayOfWeek(new DaysOfWeek(this.Session).Monday)
                .WithNextExecutionDate(mayFourteen2018)
                .WithFinalExecutionDate(mayFourteen2018.AddDays(21))
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            RepeatingSalesInvoices.Daily(this.Session);

            Assert.Equal(countBefore + 1, new SalesInvoices(this.Session).Extent().Count);
            Assert.Equal(new DateTime(2018, 5, 21), repeatingInvoice.NextExecutionDate.Date);
            Assert.Equal(new DateTime(2018, 5, 14), repeatingInvoice.PreviousExecutionDate.Value.Date);
            Assert.Single(repeatingInvoice.SalesInvoices);

            var mayTwentyOne2018 = new DateTime(2018, 5, 21, 12, 0, 0, DateTimeKind.Utc);
            timeShift = mayTwentyOne2018 - DateTime.UtcNow;
            this.TimeShift = timeShift;

            Assert.False(this.Session.Derive(false).HasErrors);

            RepeatingSalesInvoices.Daily(this.Session);

            Assert.Equal(countBefore + 2, new SalesInvoices(this.Session).Extent().Count);
            Assert.Equal(new DateTime(2018, 5, 28), repeatingInvoice.NextExecutionDate.Date);
            Assert.Equal(2, repeatingInvoice.SalesInvoices.Count);

            var mayTwentyEight2018 = new DateTime(2018, 5, 28, 12, 0, 0, DateTimeKind.Utc);
            timeShift = mayTwentyEight2018 - DateTime.UtcNow;
            this.TimeShift = timeShift;

            Assert.False(this.Session.Derive(false).HasErrors);

            RepeatingSalesInvoices.Daily(this.Session);

            Assert.Equal(countBefore + 3, new SalesInvoices(this.Session).Extent().Count);
            Assert.Equal(new DateTime(2018, 6, 4), repeatingInvoice.NextExecutionDate.Date);
            Assert.Equal(new DateTime(2018, 5, 28), repeatingInvoice.PreviousExecutionDate.Value.Date);
            Assert.Equal(3, repeatingInvoice.SalesInvoices.Count);

            var juneFour2018 = new DateTime(2018, 6, 4, 12, 0, 0, DateTimeKind.Utc);
            timeShift = juneFour2018 - DateTime.UtcNow;
            this.TimeShift = timeShift;

            Assert.False(this.Session.Derive(false).HasErrors);

            RepeatingSalesInvoices.Daily(this.Session);

            Assert.Equal(countBefore + 4, new SalesInvoices(this.Session).Extent().Count);
            Assert.Equal(new DateTime(2018, 6, 4), repeatingInvoice.NextExecutionDate.Date);
            Assert.Equal(new DateTime(2018, 6, 4), repeatingInvoice.PreviousExecutionDate.Value.Date);
            Assert.Equal(4, repeatingInvoice.SalesInvoices.Count);

            var juneEleven2018 = new DateTime(2018, 6, 11, 12, 0, 0, DateTimeKind.Utc);
            timeShift = juneEleven2018 - DateTime.UtcNow;
            this.TimeShift = timeShift;

            Assert.False(this.Session.Derive(false).HasErrors);

            RepeatingSalesInvoices.Daily(this.Session);

            Assert.Equal(countBefore + 4, new SalesInvoices(this.Session).Extent().Count);
            Assert.Equal(new DateTime(2018, 6, 4), repeatingInvoice.NextExecutionDate.Date);
            Assert.Equal(new DateTime(2018, 6, 4), repeatingInvoice.PreviousExecutionDate.Value.Date);
            Assert.Equal(4, repeatingInvoice.SalesInvoices.Count);
        }
    }
}
