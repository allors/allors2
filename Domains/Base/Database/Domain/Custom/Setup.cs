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
            var john = new PersonBuilder(this.session).WithFirstName("John").WithLastName("Doe").WithUserName("john@doe.org").Build();
            var jane = new PersonBuilder(this.session).WithFirstName("Jane").WithLastName("Doe").WithUserName("jane@doe.org").Build();
            var jenny = new PersonBuilder(this.session).WithFirstName("Jenny").WithLastName("Doe").Build();

            john.SetPassword("john");
            jane.SetPassword("jane");
            jenny.SetPassword("jenny");

            var acme = new OrganisationBuilder(this.session)
                .WithName("Acme")
                .WithOwner(jane)
                .WithEmployee(john)
                .WithEmployee(jenny)
                .Build();

            for (var i = 0; i < 100; i++)
            {
                new OrganisationBuilder(this.session)
                    .WithName($"Organisation-{i}")
                    .WithOwner(john)
                    .WithEmployee(jenny)
                    .WithEmployee(jane)
                    .Build();
            }

            new UserGroups(this.session).Administrators.AddMember(john);

            // Create cycles between Organisation and Person
            var cycleOrganisation1 = new OrganisationBuilder(this.session).WithName("Organisatin Cycle One").Build();
            var cycleOrganisation2 = new OrganisationBuilder(this.session).WithName("Organisatin Cycle Two").Build();

            var cyclePerson1 = new PersonBuilder(this.session).WithFirstName("Person Cycle").WithLastName("One").WithUserName("cycle@one.org").Build();
            var cyclePerson2 = new PersonBuilder(this.session).WithFirstName("Person Cycle").WithLastName("Two").WithUserName("cycle@one.org").Build();

            // One
            cycleOrganisation1.CycleOne = cyclePerson1;
            cyclePerson1.CycleOne = cycleOrganisation1;

            cycleOrganisation2.CycleOne = cyclePerson2;
            cyclePerson2.CycleOne = cycleOrganisation2;

            // Many
            cycleOrganisation1.AddCycleMany(cyclePerson1);
            cycleOrganisation1.AddCycleMany(cyclePerson2);

            cycleOrganisation1.AddCycleMany(cyclePerson1);
            cycleOrganisation1.AddCycleMany(cyclePerson2);

            cyclePerson1.AddCycleMany(cycleOrganisation1);
            cyclePerson1.AddCycleMany(cycleOrganisation2);

            cyclePerson2.AddCycleMany(cycleOrganisation1);
            cyclePerson2.AddCycleMany(cycleOrganisation2);
        }
    }
}