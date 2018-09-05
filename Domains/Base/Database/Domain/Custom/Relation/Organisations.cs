// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organisations.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System;

    using Allors.Data;
    using Allors.Meta;

    public partial class Organisations
    {
        public static readonly Guid ExtentByName = new Guid("2A2246FD-91F8-438F-B6DB-6BA9C3481778");

        public static readonly Guid FetchPeople = new Guid("F24CC434-8CDE-4E64-8970-4F693A606B7D");

        private UniquelyIdentifiableSticky<Organisation> sticky;

        public UniquelyIdentifiableSticky<Organisation> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<Organisation>(this.Session));

        protected override void CustomSetup(Setup setup)
        {
            var extentByName = new PreparedExtentBuilder(this.Session).WithUniqueId(ExtentByName).WithContent("Organisation by name").Build();
            extentByName.Extent = new Filter(this.Meta.Class)
            {
                Predicate = new Equals(this.Meta.Name)
                {
                    Parameter = "name"
                }
            };

            var fetchPeople = new PreparedFetchBuilder(this.Session).WithUniqueId(FetchPeople).WithContent("Fetch associated people").Build();
            fetchPeople.Fetch = new Fetch
            {
                Include = new Tree(M.Organisation.Class)
                    .Add(M.Organisation.Owner)
                    .Add(M.Organisation.Employees)
            };
        }

        protected override void CustomSecure(Security config)
        {
            base.CustomSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };
            config.GrantAdministrator(this.ObjectType, full);
            config.GrantCreator(this.ObjectType, full);

            config.GrantGuest(this.ObjectType, Operations.Read);
        }
    }
}
