// <copyright file="InvoiceItemType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("26f60d84-0659-4874-9c00-d6f3db11f073")]
    #endregion
    public partial class InvoiceItemType : Enumeration
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("61ab84cf-b3ac-49ab-a54b-4e57b280dc70")]
        [AssociationId("66d29ed1-0ea2-480a-939f-766476944c7e")]
        [RoleId("c2c3cac8-96b9-430d-911d-474483435fe6")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal MaxQuantity { get; set; }

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
