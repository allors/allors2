using System.Linq;
using Allors.Meta;

namespace Allors.Domain
{
    using Allors;
    using Allors.Domain;
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
        public void AnyOtherClass()
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