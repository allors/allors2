// <copyright file="ShipmentItemBilling.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("F54CE592-6935-401C-B341-198FD2E7888D")]
    #endregion
    public partial class ShipmentItemBilling : Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("46852AB7-2B72-433E-B029-404607A603CE")]
        [AssociationId("C4296EE4-9BF1-4ABB-8CB1-06DB45B66294")]
        [RoleId("B5B4CFCE-2986-4E47-A045-3D1C4F23ABE1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public ShipmentItem ShipmentItem { get; set; }

        #region Allors
        [Id("331685BD-1903-419F-A964-DF7A1F725B69")]
        [AssociationId("6200B5D0-13BF-441B-AB8D-D3F47BC922D6")]
        [RoleId("03BD9158-E9B2-4808-B598-C6BEF40CA705")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public InvoiceItem InvoiceItem { get; set; }

        #region Allors
        [Id("A1FB2C5B-7F15-4C4F-A790-752552114C0E")]
        [AssociationId("2D6985FB-48D9-45D3-90F4-19F99EDAE457")]
        [RoleId("76BF152D-CB48-42BF-8186-00F5EF292630")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }

        #region Allors
        [Id("016E7DBF-73FA-4F3E-96CD-677A34968CE6")]
        [AssociationId("FAA33C83-1276-4C50-B1A1-B4330ACF0A9F")]
        [RoleId("45A2AB33-B47A-44AA-801C-60AFEF0E8113")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }

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
