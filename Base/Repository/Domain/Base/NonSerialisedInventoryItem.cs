// <copyright file="NonSerialisedInventoryItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5b294591-e20a-4bad-940a-27ae7b2f8770")]
    #endregion
    public partial class NonSerialisedInventoryItem : InventoryItem
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public Part Part { get; set; }

        public string Name { get; set; }

        public string PartDisplayName { get; set; }

        public Lot Lot { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public Facility Facility { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string SearchString { get; set; }

        #endregion

        #region ObjectStates
        #region NonSerialisedInventoryItemState
        #region Allors
        [Id("35D3FF5B-AA47-41F9-A44F-7809EC2D7955")]
        [AssociationId("EBE15EA6-05F8-4EC0-8CD5-E5773A836EC4")]
        [RoleId("624BCFA2-C488-4404-ACFA-8D4EC7CC1B7D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public NonSerialisedInventoryItemState PreviousNonSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("4524D9FF-A484-49BD-B8BC-74C4D488FDC3")]
        [AssociationId("43452B62-BDD8-41C9-85DA-EE9DF093A917")]
        [RoleId("1F6FAA52-BC38-400D-BC04-9D7E0499F9AD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public NonSerialisedInventoryItemState LastNonSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("B31DEEC8-709E-4049-989A-D4BD3028A166")]
        [AssociationId("D3D5E468-4F4C-4EFE-822F-C9CA753C0CA6")]
        [RoleId("731CBA99-ABD0-4C7A-A38A-B606E4E42812")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public NonSerialisedInventoryItemState NonSerialisedInventoryItemState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("4E2486A2-3CF9-4EB6-B675-6565A64116A6")]
        [AssociationId("1E1295F2-DFE0-407C-A625-B6A6972251E0")]
        [RoleId("AE86A6A6-B5D9-459A-9DF9-ECFDC5C90700")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public NonSerialisedInventoryItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("53B35269-EF6C-45EE-BE20-FCDC732CE06E")]
        [AssociationId("EBE26248-2154-4F81-B8D3-00628C504A95")]
        [RoleId("5DF2F337-C6FE-4250-B513-D5BB7E13579C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public NonSerialisedInventoryItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("4a0dc5cb-8d4a-479a-8413-4df6d9e884fe")]
        [AssociationId("bffcf16e-02e3-4fc2-b940-f9e1a7f1250d")]
        [RoleId("50ae435b-260a-4a63-921d-cdce462c7c3c")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityOnHand { get; set; }

        #region Allors
        [Id("c02a2b4a-5ae4-4050-a975-4e675fa56589")]
        [AssociationId("26e01b4a-f933-452b-8686-e0eb2382217c")]
        [RoleId("0050e269-108d-4a9f-ba84-9360d6cd42e2")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AvailableToPromise { get; set; }

        #region Allors
        [Id("2d25feaf-3468-41d5-8107-ce46b78a82b4")]
        [AssociationId("270a9e95-0bf5-4a61-b48b-984a2dd8f7ac")]
        [RoleId("dbdf2ecd-4b04-4253-8914-7295a13b32db")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityCommittedOut { get; set; }

        #region Allors
        [Id("7d402c70-da15-4f28-917b-e75e3fd22560")]
        [AssociationId("7f61067b-af9c-4f42-8555-df490b9b064d")]
        [RoleId("079ce710-e2f5-45b3-92ad-a363c762540a")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityExpectedIn { get; set; }

        #region Allors
        [Id("ba5e2476-abdd-4d61-8a14-5d99a36c4544")]
        [AssociationId("f1e3216e-1af7-4354-b8ac-258bfa9222ac")]
        [RoleId("4d41e84c-ee79-4ce2-874e-a000e30c1120")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal PreviousQuantityOnHand { get; set; }

        #region Allors
        [Id("F1986E5F-3A8F-478A-B4C4-C6913C78AFE2")]
        [AssociationId("EE669F73-E91F-4559-81A9-170265872592")]
        [RoleId("F4515CDF-FC20-4BD5-8F6E-81583792E20E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public NonSerialisedInventoryItemState InventoryItemState { get; set; }

        #region Allors
        [Id("9865c082-839f-406b-8973-8bc57ca6da5f")]
        [AssociationId("c2cc3d1f-c054-4401-b57b-8e1278407e67")]
        [RoleId("dc26b816-71d6-416f-8e46-b5c2eefea3e8")]
        [Indexed]
        [Size(256)]
        #endregion
        [Workspace]
        public string PartLocation { get; set; }

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
