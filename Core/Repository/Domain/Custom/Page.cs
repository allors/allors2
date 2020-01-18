// <copyright file="Place.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("0777C78C-CB50-4FDD-8386-5BCEC00B208C")]
    #endregion
    public partial class Page : UniquelyIdentifiable
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("9B2F32B4-DF88-41DA-AE4C-A7A8D4232C1C")]
        [AssociationId("D972F9FF-EF5E-4DEA-B37A-C4A4970B52D1")]
        [RoleId("69DC7104-EC8B-49A9-834C-6331B1CD5BFA")]
        [Indexed]
        #endregion
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("E3117BBB-3B1E-465A-8DD0-CC5FE3A5A905")]
        [AssociationId("EA487B29-9BE5-455B-B6EE-1670F37AF4BB")]
        [RoleId("4B2AA21D-D54B-4EAF-92A4-64651C2692B8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public Media Content { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
