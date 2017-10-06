// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Persons.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;

namespace Allors.Domain
{
    using System.Collections.Generic;

    using Meta;

    public partial class People
    {
        public static readonly Guid AdministratorId = new Guid("FF791BA1-6E02-4F64-83A3-E6BEE1208C11");
        public static readonly Guid GuestId = new Guid("1261CB56-67F2-4725-AF7D-604A117ABBEC");

        public Extent<Person> Employees
        {
            get
            {
                var employees = new People(this.Session).Extent();
                var employeeRole = new PersonRoles(this.Session).Employee;
                employees.Filter.AddContains(M.Person.PersonRoles, employeeRole);
                return employees;
            }
        }

        public static void AppsOnDeriveCommissions(ISession session)
        {
            foreach (Person person in session.Extent<Person>())
            {
                if (person.ExistSalesRepRevenuesWhereSalesRep)
                {
                    person.AppsOnDeriveCommission();
                }
            }
        }

        protected override void AppsSetup(Setup config)
        {
            var users = new Users(this.Session).Extent();
            foreach (Person person in users)
            {
                person.AddPersonRole(new PersonRoles(this.Session).Employee);
            }
        }

        protected override void AppsPrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.Role);
            setup.AddDependency(this.Meta.ObjectType, M.PersonRole);
            setup.AddDependency(this.Meta.ObjectType, M.InternalOrganisation);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };
            config.GrantOwner(this.ObjectType, full);
        }
    }
}