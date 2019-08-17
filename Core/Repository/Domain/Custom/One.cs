// <copyright file="One.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("5d9b9cad-3720-47c3-9693-289698bf3dd0")]
    #endregion
    public partial class One : Object, Shared
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("448878af-c992-4256-baa7-239335a26bc6")]
        [AssociationId("2c9236ed-892e-4005-9730-5a14f03f71e1")]
        [RoleId("355b2e85-e597-4f88-9dca-45cbfbde527f")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public Two Two { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}
