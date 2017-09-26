//------------------------------------------------------------------------------------------------- 
// <copyright file="EmploymentTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Meta;

    public class EmploymentTests : DomainTest
    {
        private Person employee;
        private Singleton internalOrganisation;

        public EmploymentTests()
        {
            this.employee = new PersonBuilder(this.DatabaseSession).WithLastName("slave").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            this.internalOrganisation = Singleton.Instance(this.DatabaseSession);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
        }
    }
}
