// <copyright file="SerialisedItemCharacteristicType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5A0B6477-7B54-48FA-AF59-7B664587F197")]
    #endregion
    public partial class SerialisedItemCharacteristicType : Enumeration
    {
        #region inherited properties

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("A141BDEF-06EF-4441-BACE-C5E3B42F755C")]
        [AssociationId("7D8146E2-78C6-4D10-81BA-13BF6F474F0A")]
        [RoleId("C18F7738-0988-4173-9B64-9447CC50767D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public IUnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("E15CA0FB-5C27-471D-A803-D24021E42B2E")]
        [AssociationId("2419AB63-BF7A-4261-99A8-090C94752661")]
        [RoleId("6486D3B2-AE24-441F-A9E2-3B1B7083032A")]
        [Indexed]
        #endregion
        [Workspace]
        [Required]
        public bool IsPublic { get; set; }

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
        [Id("642DC9D7-E6D8-45A5-8109-B80013C6CF32")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}
