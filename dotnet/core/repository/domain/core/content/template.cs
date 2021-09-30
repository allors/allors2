// <copyright file="Template.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("93F8B97B-2D9A-42FC-A823-7BDCC5A92203")]
    #endregion
    public partial class Template : UniquelyIdentifiable, Deletable, Object
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("64DD490F-2B13-4D63-94A4-6BCE96FA14C2")]
        [AssociationId("6A7D9713-92F1-41D6-B082-FD30FB247AA0")]
        [RoleId("ADE2850A-0F0E-410F-B756-81F4928B703B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        public TemplateType TemplateType { get; set; }

        #region Allors
        [Id("93C9C5F2-7D0B-475D-80B8-7CAC7B11CCDA")]
        [AssociationId("E5E4E397-E9D6-4E21-87D5-0A6BEF0431E4")]
        [RoleId("A915962A-91B2-4EC0-A17D-1FA32AA239E1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Required]
        public Media Media { get; set; }

        #region Allors
        [Id("3BC9EEAE-717F-4030-88ED-68057B14ACEC")]
        [AssociationId("8B04F22C-48EC-469F-BBD0-1136AF8325A3")]
        [RoleId("786036A9-D4F7-449E-BC23-85D512CC53D3")]
        [Indexed]
        #endregion
        [Required]
        public string Arguments { get; set; }

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
