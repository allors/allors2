// --------------------------------------------------------------------------------------------------------------------
// <copyright file="People.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System;

    using Meta;

    public partial class People
    {
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
            var internalOrganisations = this.Session.Extent<Organisation>();
            internalOrganisations.Filter.AddEquals(M.Organisation.IsInternalOrganisation, true);

            var users = new Users(this.Session).Extent();

            foreach (Person person in users)
            {
                foreach (InternalOrganisation internalOrganisation in internalOrganisations)
                {
                    new EmploymentBuilder(this.Session).WithEmployer(internalOrganisation).WithEmployee(person).Build();
                }
            }
        }

        protected override void AppsPrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.Role);
            setup.AddDependency(this.Meta.ObjectType, M.PersonRole);
            setup.AddDependency(this.Meta.ObjectType, M.InternalOrganisation);
            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
            setup.AddDependency(this.ObjectType, M.ContactMechanismPurpose.ObjectType);
            setup.AddDependency(this.ObjectType, M.InternalOrganisation.ObjectType);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantOwner(this.ObjectType, full);
        }
    }
}