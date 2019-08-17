// <copyright file="SerialisedInventoryItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("d042eeae-5c17-4936-861b-aaa9dfaed254")]
    #endregion
    public partial class SerialisedInventoryItemState : ObjectState
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] ObjectDeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("A38DDADC-22A1-4606-9059-2D803F3F4EF1")]
        [AssociationId("DEF7CEF2-A688-4B0C-A91C-922C665485D0")]
        [RoleId("EF144C5E-C273-4D23-B863-21F17E7996C3")]
        #endregion
        [Indexed]
        [Workspace]
        public bool IsActive { get; set; }
    }
}
