// <copyright file="QuoteItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("6D38838C-CA7A-4ACC-B240-E4A1F3AE2DC9")]
    #endregion
    public partial class QuoteItemVersion : Version
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("EB4A6E73-333A-4BBD-BE8A-C7DCCFCC7A8A")]
        [AssociationId("DA96FB4C-B16E-465C-8152-08C1C8BFD996")]
        [RoleId("2C42C31F-FBC8-4686-80FB-8341AEC1D3CD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        public QuoteItemState QuoteItemState { get; set; }

        #region Allors
        [Id("5A4AFCB5-B067-424D-95D7-B8B77AB9D125")]
        [AssociationId("9FA8C5B7-B6F2-46DE-BA30-3C95C1EDFFE3")]
        [RoleId("821D236D-6E52-43CA-AE36-ECC82AB3CF31")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string InternalComment { get; set; }

        #region Allors
        [Id("2FF532EE-F6C4-4DE9-9F6A-53EBB0747D51")]
        [AssociationId("EAA0062D-94ED-491D-BA2B-926CB538A306")]
        [RoleId("282B4962-9587-4F85-822A-BA60BF4E9E10")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Authorizer { get; set; }

        #region Allors
        [Id("36035A65-0806-4B96-9E57-5AC0176DA4C2")]
        [AssociationId("31E9B43D-C761-4070-B9A7-B2FBE20E87A4")]
        [RoleId("B112F8B9-9F62-40DD-9B4D-53B0BD05392F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Deliverable Deliverable { get; set; }

        #region Allors
        [Id("95CF9D84-E1FE-40DC-AE7C-8CA2DDC6687C")]
        [AssociationId("B7D60263-AB58-4857-A1EF-C9BE6E4ABAFE")]
        [RoleId("44338CE0-5468-4DEC-8900-3FDC3BFDE859")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("A24339CD-58D1-4849-AB1C-CA38543B5580")]
        [AssociationId("B5877AB4-9BAB-4286-952E-C03658A89686")]
        [RoleId("8AA79351-69B2-4C64-91CB-B51F07AC3402")]
        #endregion
        [Workspace]
        public DateTime EstimatedDeliveryDate { get; set; }

        #region Allors
        [Id("4D54E4C2-E612-484C-B4A4-EAC04D96BD5A")]
        [AssociationId("0ABDB742-C720-4115-94A6-36F9AD074309")]
        [RoleId("8B313C23-AFB8-463A-904A-AEED5F4D2029")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Workspace]
        public DateTime RequiredByDate { get; set; }

        #region Allors
        [Id("9E8A5EBF-AE58-4F04-B29C-2A813713C52E")]
        [AssociationId("D551AED3-2DB8-4D9F-9D00-CD8732B350BD")]
        [RoleId("08AB1CC0-63ED-4BD4-8CE9-32A5105959B4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("9BD1DB0E-5215-4764-8C1D-C88B98917D5B")]
        [AssociationId("9060BCC5-ABEF-464A-8D76-71D19AD0359F")]
        [RoleId("2004FDF9-242A-4B12-A24C-C794FAB231C0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("14e62d23-d178-4445-81f2-f984447080d8")]
        [AssociationId("625b0782-8fa7-4994-aea7-bde096766d4d")]
        [RoleId("03c3531f-7fde-4096-9ae6-15fdcd4aa56a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("985322DB-5906-44FE-AD94-A4243AD99ADC")]
        [AssociationId("D07F2E38-9AB1-46D8-BEA7-EBD22B1F2E96")]
        [RoleId("DD09235F-76A8-4C41-8E20-37AD30347602")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal UnitPrice { get; set; }

        #region Allors
        [Id("FCA1614D-45CD-4295-9911-B9464EA9A4C4")]
        [AssociationId("3AF6A062-A8C0-4222-BF53-06E044414AA7")]
        [RoleId("E9920620-21A9-4582-8AA7-C37762ED6008")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Skill Skill { get; set; }

        #region Allors
        [Id("E4C53A94-52F4-4A0D-8204-B315EECB16E2")]
        [AssociationId("684DEC65-BEA3-41DA-B396-D74947EEA74F")]
        [RoleId("E7FD5244-7767-49FF-AFA1-43C9AF2034A4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public WorkEffort WorkEffort { get; set; }

        #region Allors
        [Id("932A2BF0-AA86-431B-BC77-818E1EC5A837")]
        [AssociationId("CC440663-5290-490D-AF9D-6135025BAF6C")]
        [RoleId("DC2DE387-633E-4885-8C23-EE855B93BD60")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public QuoteTerm[] QuoteTerms { get; set; }

        #region Allors
        [Id("A8810D49-AAA6-43A8-99F8-DC7E6B30D83E")]
        [AssociationId("01BC3F91-C5C1-4B05-B0BD-64F23E52D879")]
        [RoleId("CB43411F-3319-42F6-BC80-1E95A2E0A316")]
        #endregion
        [Workspace]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("24B8A678-F28D-4041-9514-490CDC1FDE7D")]
        [AssociationId("1D660EE6-F4A5-4F4A-B6F0-1B51BC340C7F")]
        [RoleId("5C7FC5A1-11E4-43A1-A768-063620112099")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public RequestItem RequestItem { get; set; }

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
