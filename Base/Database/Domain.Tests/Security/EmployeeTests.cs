// <copyright file="EmployeeTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Allors;
    using Xunit;

    public class EmployeeSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void Person()
        {
            var employee = new Employments(this.Session).Extent().Select(v => v.Employee).First();
            this.SetIdentity(employee.UserName);

            var acl = new AccessControlList(employee, employee);
            Assert.True(acl.CanRead(M.Person.FirstName));
            Assert.False(acl.CanWrite(M.Person.FirstName));
        }

        [Fact]
        public void Good()
        {
            var good = new Goods(this.Session).Extent().First();

            var employee = new Employments(this.Session).Extent().Select(v => v.Employee).First();
            this.SetIdentity(employee.UserName);

            var acl = new AccessControlList(good, employee);
            Assert.True(acl.CanRead(M.Good.Name));
            Assert.False(acl.CanWrite(M.Good.Name));
        }

        [Fact]
        public void WorkTaskNewInSession()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("worktask").WithCustomer(customer).Build();

            this.Session.Derive();

            var employee = new Employments(this.Session).Extent().Select(v => v.Employee).First();
            this.SetIdentity(employee.UserName);

            Assert.True(workTask.Strategy.IsNewInSession);

            var acl = new AccessControlList(workTask, employee);
            Assert.False(acl.CanRead(M.WorkTask.Name));
            Assert.False(acl.CanWrite(M.WorkTask.Name));
        }

        [Fact]
        public void WorkTask()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workTask = new WorkTaskBuilder(this.Session).WithName("worktask").WithCustomer(customer).Build();

            this.Session.Derive();
            this.Session.Commit();

            var employee = new Employments(this.Session).Extent().Select(v => v.Employee).First();
            this.SetIdentity(employee.UserName);

            Assert.False(workTask.Strategy.IsNewInSession);

            var acl = new AccessControlList(workTask, employee);
            Assert.False(acl.CanRead(M.WorkTask.Name));
            Assert.False(acl.CanWrite(M.WorkTask.Name));
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

            var employee = new Employments(this.Session).Extent().Select(v => v.Employee).First();
            this.SetIdentity(employee.UserName);

            Assert.True(salesInvoice.Strategy.IsNewInSession);

            var acl = new AccessControlList(salesInvoice, employee);
            Assert.False(acl.CanRead(M.SalesInvoice.Description));
            Assert.False(acl.CanWrite(M.SalesInvoice.Description));

            this.Session.Commit();

            Assert.False(salesInvoice.Strategy.IsNewInSession);

            acl = new AccessControlList(salesInvoice, employee);
            Assert.False(acl.CanRead(M.SalesInvoice.Description));
            Assert.False(acl.CanWrite(M.SalesInvoice.Description));
        }

        [Fact]
        public void UserGroup()
        {
            var userGroup = new UserGroups(this.Session).Administrators;

            var employee = new Employments(this.Session).Extent().Select(v => v.Employee).First();
            this.SetIdentity(employee.UserName);

            var acl = new AccessControlList(userGroup, employee);
            Assert.True(acl.CanRead(M.UserGroup.Members));
            Assert.False(acl.CanWrite(M.UserGroup.Members));
        }

        [Fact]
        public void Singleton()
        {
            var employee = new Employments(this.Session).Extent().Select(v => v.Employee).First();
            this.SetIdentity(employee.UserName);

            var acl = new AccessControlList(this.Session.GetSingleton(), employee);
            Assert.True(acl.CanRead(M.Singleton.SalesAccountManagerUserGroup));
            Assert.False(acl.CanWrite(M.Singleton.SalesAccountManagerUserGroup));
        }
    }
}
