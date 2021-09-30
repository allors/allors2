// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class Organisations
    {
        private UniquelyIdentifiableSticky<Organisation> cache;

        public UniquelyIdentifiableSticky<Organisation> Cache => this.cache ??= new UniquelyIdentifiableSticky<Organisation>(this.Session);

        protected override void CustomPrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.Restriction);

        protected override void CustomSecure(Security security)
        {
            var restrictions = new Restrictions(this.Session);
            var permissions = new Permissions(this.Session);

            restrictions.ToggleRestriction.DeniedPermissions = new[]
            {
                permissions.Get(this.Meta.Class, this.Meta.Name, Operations.Write),
                permissions.Get(this.Meta.Class, this.Meta.Owner, Operations.Write),
                permissions.Get(this.Meta.Class, this.Meta.Employees, Operations.Write),
            };
        }
    }
}
