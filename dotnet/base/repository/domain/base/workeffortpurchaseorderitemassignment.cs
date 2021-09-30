// <copyright file="WorkEffortPurchaseOrderItemAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("2C8554EF-7B0E-47A3-AC66-E6CB50E20DF9")]
    #endregion
    public partial class WorkEffortPurchaseOrderItemAssignment : Deletable, DelegatedAccessControlledObject
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("37D28124-9D29-435C-838F-7B043DCFFF33")]
        [AssociationId("BD19B60D-CEA9-4128-9361-25621B211EA7")]
        [RoleId("2D468085-52AB-4262-9806-1BAE7BAA1B37")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public WorkEffort Assignment { get; set; }

        #region Allors
        [Id("C314C1F1-1E49-4CC5-9915-77ABCB41D685")]
        [AssociationId("01A65A62-4FFB-49AA-A350-5507C42D224F")]
        [RoleId("27DDB132-C3AE-44FB-99C2-6A9C7CFF0250")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public PurchaseOrderItem PurchaseOrderItem { get; set; }

        #region Allors
        [Id("95577BE1-EC7A-41D2-B51F-96791B172B84")]
        [AssociationId("A18A9971-031F-47F2-95F5-81676C6ECB98")]
        [RoleId("FCB518DC-3510-4113-BCFF-38C3FEE4C6AE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public PurchaseOrder PurchaseOrder { get; set; }

        #region Allors
        [Id("346A4EC3-16F7-45EB-9BD6-949EDB86BCFF")]
        [AssociationId("377449FA-8E79-4163-8CEB-DDE4B816264D")]
        [RoleId("1EF8D82F-9E88-416E-8C32-7D18F5056408")]
        #endregion
        [Required]
        [Workspace]
        public int Quantity { get; set; }

        #region Allors
        [Id("B4697583-DBD5-4713-AF01-50093D3BDD6E")]
        [AssociationId("4B5EFC62-49F6-440F-B6F9-ED3B8AB6FB61")]
        [RoleId("A3AB4ADE-4267-41F0-9370-B2AA70EB6B42")]
        #endregion
        [Precision(19)]
        [Scale(4)]
        [Workspace]
        public decimal AssignedUnitSellingPrice { get; set; }

        #region Allors
        [Id("7EFCD991-929F-4A0E-A066-36A0BD4C045D")]
        [AssociationId("F4F54710-9573-44BE-9C81-E63E4693D262")]
        [RoleId("8CEACD50-293C-44F5-9D54-80BE95DD26D9")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal UnitPurchasePrice { get; set; }

        #region Allors
        [Id("017BC7F2-A9A6-46F4-A4EE-B47F802D7BB2")]
        [AssociationId("A0998071-8DA3-40F6-B99C-F1C4BCF6ABC8")]
        [RoleId("F0AF2361-E893-4318-ADEA-CBFAB6F4035B")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal UnitSellingPrice { get; set; }

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

        public void DelegateAccess() { }

        #endregion

        #region Allors

        [Id("99e7aefd-9a70-4fc2-ad3d-a8606bd6ba3d")]
        #endregion
        public void CalculateSellingPrice()
        {
        }
    }
}
