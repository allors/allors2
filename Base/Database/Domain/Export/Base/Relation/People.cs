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
    using Meta;

    public partial class People
    {
        protected override void BaseSetup(Setup setup)
        {
            var internalOrganisations = new Organisations(this.Session).InternalOrganisations();
            
            var users = new Users(this.Session).Extent();

            foreach (Person person in users)
            {
                foreach (var internalOrganisation in internalOrganisations)
                {
                    new EmploymentBuilder(this.Session).WithEmployer(internalOrganisation).WithEmployee(person).Build();
                }
            }
        }

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.Role);
            setup.AddDependency(this.Meta.ObjectType, M.PersonRole);
            setup.AddDependency(this.Meta.ObjectType, M.InternalOrganisation);
            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
            setup.AddDependency(this.ObjectType, M.ContactMechanismPurpose.ObjectType);
            setup.AddDependency(this.ObjectType, M.InternalOrganisation.ObjectType);
        }

        protected override void BaseSecure(Security config)
        {
            

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantOwner(this.ObjectType, full);
        }
    }
}