using System.Linq;
using Allors.Meta;

namespace Allors.Domain
{
    using Allors;
    using Xunit;
   
    public class EmployeeTests : DomainTest
    {
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
    }
}