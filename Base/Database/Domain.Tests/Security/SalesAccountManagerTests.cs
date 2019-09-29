// <copyright file="SalesAccountManagerTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors;
    using Allors.Meta;
    using Xunit;

    public class SalesAccountManagerSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void Person()
        {
            var person = new People(this.Session).Extent().First();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            var singleton = this.Session.GetSingleton();

            var salesaccm = new PersonBuilder(this.Session)
                .WithUserName("salesaccountmanager")
                .WithLastName("salesaccm")
                .Build();

            singleton.AddSalesAccountManager(salesaccm);
            new EmploymentBuilder(this.Session).WithEmployee(salesaccm).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(salesaccm);

            this.Session.Derive(true);

            this.SetIdentity(salesaccm.UserName);

            var acl = new AccessControlLists(salesaccm)[person];
            Assert.True(acl.CanRead(M.Person.FirstName));
            Assert.True(acl.CanWrite(M.Person.FirstName));
        }

        [Fact]
        public void SalesInvoice()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("Mechelen")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var salesInvoice = new SalesInvoiceBuilder(this.Session).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            this.Session.Derive();

            var singleton = this.Session.GetSingleton();

            var salesaccm = new PersonBuilder(this.Session)
                .WithUserName("salesaccountmanager")
                .WithLastName("salesaccm")
                .Build();

            singleton.AddSalesAccountManager(salesaccm);
            new EmploymentBuilder(this.Session).WithEmployee(salesaccm).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(salesaccm);

            this.Session.Derive(true);

            this.SetIdentity(salesaccm.UserName);

            Assert.True(salesInvoice.Strategy.IsNewInSession);

            var acl = new AccessControlLists(salesaccm)[salesInvoice];
            Assert.True(acl.CanRead(M.SalesInvoice.Description));
            Assert.True(acl.CanWrite(M.SalesInvoice.Description));

            this.Session.Commit();

            Assert.False(salesInvoice.Strategy.IsNewInSession);

            acl = new AccessControlLists(salesaccm)[salesInvoice];
            Assert.True(acl.CanRead(M.SalesInvoice.Description));
            Assert.True(acl.CanWrite(M.SalesInvoice.Description));
        }

        [Fact]
        public void Good()
        {
            var good = new Goods(this.Session).Extent().First();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            var singleton = this.Session.GetSingleton();

            var salesaccm = new PersonBuilder(this.Session)
                .WithUserName("salesaccountmanager")
                .WithLastName("salesaccm")
                .Build();

            singleton.AddSalesAccountManager(salesaccm);
            new EmploymentBuilder(this.Session).WithEmployee(salesaccm).WithEmployer(internalOrganisation).Build();

            var userGroups = new UserGroups(this.Session);
            userGroups.Creators.AddMember(salesaccm);

            this.Session.Derive(true);

            this.SetIdentity(salesaccm.UserName);

            var acl = new AccessControlLists(salesaccm)[good];
            Assert.True(acl.CanRead(M.Good.Name));
            Assert.False(acl.CanWrite(M.Good.Name));
        }
    }
}
