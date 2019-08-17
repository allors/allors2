//-------------------------------------------------------------------------------------------------
// <copyright file="Currency.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("fd397adf-40b4-4ef8-b449-dd5a24273df3")]
    #endregion
    [Plural("Currencies")]
    public partial class Currency : Enumeration, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public string Name { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public bool IsActive { get; set; }

        #endregion

        #region Allors
        [Id("294a4bdc-f03a-47a2-a649-419e6b9021a3")]
        [AssociationId("f9eec7c6-c4cd-4d8c-a5f7-44855328cd7e")]
        [RoleId("09d74027-4457-4788-803c-24b857245565")]
        #endregion
        [Required]
        [Unique]
        [Size(256)]
        [Workspace]
        public string IsoCode { get; set; }

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
