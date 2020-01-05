// <copyright file="UserProfile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("EF12372D-2A94-406C-AE3F-18E9CF2FD016")]
    #endregion
    public partial class UserProfile : Deletable
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("5EF420B6-6BA6-4913-87F3-D5EBC40CB794")]
        [AssociationId("CC606A1F-1D82-42CF-9547-50EC07207180")]
        [RoleId("31322198-E75B-4D6B-8A90-ECC6D7F137F2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation DefaultInternalOrganization { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
