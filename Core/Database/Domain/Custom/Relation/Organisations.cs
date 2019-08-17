// <copyright file="Organisations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
            var extentByName = new PreparedExtentBuilder(this.Session).WithUniqueId(ExtentByName).WithDescription("Organisation by name").Build();
            extentByName.Extent = new Filter(this.Meta.Class)
            {
                Predicate = new Equals(this.Meta.Name)
                {
                    Parameter = "name"
                }
            };

            var fetchPeople = new PreparedFetchBuilder(this.Session).WithUniqueId(FetchPeople).WithDescription("Fetch People").Build();
            fetchPeople.Fetch = new Fetch
            {
                Include = new Tree(M.Organisation.Class)
                    .Add(M.Organisation.Owner)
                    .Add(M.Organisation.Employees)
            };
        }
    }
}
