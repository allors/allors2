// <copyright file="Singleton.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;

    public partial class Singleton
    {
        #region Allors
        [Id("94D078CB-BA58-42A4-8005-8D756DE7A463")]
        [AssociationId("337437C2-36F6-4139-9CA1-A2C71ED3393D")]
        [RoleId("21E6FDBC-834B-4038-9B2A-38B5A94067FB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Person AutocompleteDefault { get; set; }

        #region Allors
        [Id("7BA0F99D-4E79-4D55-89BE-F94B3B14E98F")]
        [AssociationId("2445B582-84E2-4CBB-A436-7155F98D48C7")]
        [RoleId("8AA95920-FE7C-458E-8131-7433F1422BA5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Person SelectDefault { get; set; }
    }
}
