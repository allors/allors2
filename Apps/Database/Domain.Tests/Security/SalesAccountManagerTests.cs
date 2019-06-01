using System.Linq;
using Allors.Meta;

namespace Allors.Domain
{
    using Allors;
    using Xunit;
   
    public class SalesAccountManagerTests : DomainTest
    {
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

            var acl = new AccessControlList(person, salesaccm);
            Assert.True(acl.CanRead(M.Person.FirstName));
            Assert.True(acl.CanWrite(M.Person.FirstName));
        }

        [Fact]
        public void SalesInvoice()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                    .WithLocality("Mechelen")
                    .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                    .Build())

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

            var acl = new AccessControlList(salesInvoice, salesaccm);
            Assert.True(acl.CanRead(M.SalesInvoice.Description));
            Assert.True(acl.CanWrite(M.SalesInvoice.Description));

            this.Session.Commit();

            Assert.False(salesInvoice.Strategy.IsNewInSession);

            acl = new AccessControlList(salesInvoice, salesaccm);
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

            var acl = new AccessControlList(good, salesaccm);
            Assert.True(acl.CanRead(M.Good.Name));
            Assert.False(acl.CanWrite(M.Good.Name));
        }
    }
}