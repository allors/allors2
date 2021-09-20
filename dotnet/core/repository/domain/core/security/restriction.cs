// <copyright file="SecurityToken.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("753A230E-6C29-4C3C-9592-323BE0778ED6")]
    #endregion
    public partial class Restriction : UniquelyIdentifiable, Deletable
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("F7F98147-FD94-4BB1-A974-6405A3AB369E")]
        [AssociationId("F3090007-F1CD-4EB9-AD2B-724D72B4196A")]
        [RoleId("EBDCAFC1-6CA3-465B-8B55-5838396CA5A5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public Permission[] DeniedPermissions { get; set; }

        #region Allors
        [Id("2848B7B7-DDBC-4A47-8D70-258DB1D90916")]
        [AssociationId("BDC9AF3A-EAC2-4F46-9A5B-90523D3C5774")]
        [RoleId("11BCE16D-AEA7-4EEC-B4BB-8CF7C3247B6B")]
        #endregion
        [Indexed]
        [Required]
        [Derived]
        public bool IsWorkspace { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }
        #endregion
    }
}
