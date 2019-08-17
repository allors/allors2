// <copyright file="VatClause.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("D9881B21-DE74-4C4B-8306-B4C10FF7D0F1")]
    #endregion
    public partial class VatClause : Enumeration
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("C8824C67-7F00-4A3D-8F1F-A12914729193")]
        [AssociationId("E0F84D0C-8540-45E4-A978-AC52FBC7D4C6")]
        [RoleId("53CA68BC-EAB3-4E5D-867B-6E68EE0F7589")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedClause { get; set; }

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
    }
}
