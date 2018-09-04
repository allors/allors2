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

    public partial class Organisations
    {
        public static readonly Guid PullByName = new Guid("2A2246FD-91F8-438F-B6DB-6BA9C3481778");

        private UniquelyIdentifiableSticky<Organisation> sticky;

        public UniquelyIdentifiableSticky<Organisation> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<Organisation>(this.Session));

        protected override void CustomSetup(Setup setup)
        {
            new PreparedPullBuilder(this.Session).WithUniqueId(PullByName).WithContent("Organisation by name").Build()
                .Pull = new Pull
                {
                    Extent = new Filter(this.Meta.Class)
                    {
                        Predicate = new Equals(this.Meta.Name)
                        {
                            Parameter = "name"
                        }
                    }
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
