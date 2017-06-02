// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setup.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;

    using Allors.Domain;

    public partial class Setup
    {
        private void CustomOnPrePrepare()
        {
        }

        private void CustomOnPostPrepare()
        {
        }

        private void CustomOnPreSetup()
        {
        }

        private void CustomOnPostSetup()
        {
            // Default access policy
            var security = new Security(session);
            foreach (var @class in session.Database.MetaPopulation.Classes)
            {
                security.GrantAdministrator(@class, Operations.Read, Operations.Write, Operations.Execute);
                security.GrantCreator(@class, Operations.Read, Operations.Write, Operations.Execute);
            }

            //var john = new PersonBuilder(this.session).WithFirstName("John").WithLastName("Doe").WithUserName("john@doe.org").Build();
            //var jane = new PersonBuilder(this.session).WithFirstName("Jane").WithLastName("Doe").WithUserName("jane@doe.org").Build();
            //var jenny = new PersonBuilder(this.session).WithFirstName("Jenny").WithLastName("Doe").Build();

            //john.SetPassword("john");
            //jane.SetPassword("jane");
            //jenny.SetPassword("jenny");

            //var acme = new OrganisationBuilder(this.session)
            //    .WithName("Acme")
            //    .WithOwner(jane)
            //    .WithEmployee(john)
            //    .WithEmployee(jenny)
            //    .Build();

            //new UserGroups(this.session).Administrators.AddMember(john);
        }
    }
}