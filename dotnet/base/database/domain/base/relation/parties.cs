// <copyright file="Parties.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Parties
    {
        public static void Daily(ISession session)
        {
            var parties = new Parties(session).Extent();

            foreach (Party party in parties)
            {
                party.DeriveRelationships();
            }
        }

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.ContactMechanismPurpose);
            setup.AddDependency(this.ObjectType, M.Settings);
        }
    }
}
