// <copyright file="OrderItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("304981F1-CCF3-4946-86E0-5DE1F929BA08")]
    #endregion
    public partial interface OrderItemVersion : PriceableVersion
    {
        #region Allors
        [Id("70F07626-DD20-4BD8-A836-66E1C3DE5EE2")]
        [AssociationId("646ABFF3-47FD-419D-B33F-5571A387BA40")]
        [RoleId("D0FE3389-66DF-4DEA-B050-6D232B769545")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("6E07F778-88EB-4A3C-8923-4FD508373C3E")]
        [AssociationId("57CDB4E0-66A5-41E2-B43B-5713D434311C")]
        [RoleId("5FD5FD85-7206-402E-B048-01241FDF9F36")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        BudgetItem BudgetItem { get; set; }

        #region Allors
        [Id("1B393511-B8B5-488C-B2AB-689C8316EC7D")]
        [AssociationId("841095A8-DA63-4975-8EB0-066145D93789")]
        [RoleId("3FFA368F-2CC9-4F7B-9602-192D645F8E93")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        decimal PreviousQuantity { get; set; }

        #region Allors
        [Id("EDD60449-DBC2-4102-BF83-48CE84C528A8")]
        [AssociationId("2A188225-B9EB-4918-ADCA-F90721A5DB8A")]
        [RoleId("F8B52F60-15C2-4858-A1F0-28C0CC42D7DB")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal QuantityOrdered { get; set; }

        #region Allors
        [Id("F5147845-F873-4B4A-B3E6-43A7448F6EAB")]
        [AssociationId("436A5B5A-37FE-41BE-9446-88B11E926507")]
        [RoleId("AD6E916C-CD42-44AA-9102-53CD1A7474E6")]
        #endregion
        [Workspace]
        [Size(-1)]
        string Description { get; set; }

        #region Allors
        [Id("EE2BC870-C91D-4B2A-A66F-7FC1633A88A6")]
        [AssociationId("FCBD0FE8-F187-4535-BD91-7CAB2EF2B251")]
        [RoleId("79AB954C-1F4E-4B84-A453-1A569B00AC7C")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        PurchaseOrder CorrespondingPurchaseOrder { get; set; }

        #region Allors
        [Id("12184E43-0B6B-4DE9-A36F-CDEAA86A7AFF")]
        [AssociationId("6A939144-6E6E-49CA-8AE9-4741F9EE5693")]
        [RoleId("A6A69F1C-0FF5-46D7-AF18-C64DAC4D9AE5")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalOrderAdjustment { get; set; }

        #region Allors
        [Id("24AD3FC1-3719-4387-A366-3F11E03F19EC")]
        [AssociationId("A9EBAE94-2C99-4C77-82D0-EA3460F87B1F")]
        [RoleId("C00F20A5-A0C4-4A87-8749-3830A134B2F3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        QuoteItem QuoteItem { get; set; }

        #region Allors
        [Id("9ED602D2-7520-414E-B699-6A9F3DE8A797")]
        [AssociationId("808BEE7D-2992-4050-866A-3BF1EE2D7298")]
        [RoleId("93CD86F6-D3F4-42DF-B6B8-F014D295FB99")]
        #endregion
        [Workspace]
        DateTime AssignedDeliveryDate { get; set; }

        #region Allors
        [Id("8303667C-ACD8-4064-9831-9C0129676AB3")]
        [AssociationId("AD21D921-817D-4B5B-BBD0-E2DF48107957")]
        [RoleId("77DEF4E0-E4FF-4A82-BEF1-7B5A65295FB2")]
        #endregion
        [Workspace]
        DateTime DerivedDeliveryDate { get; set; }

        #region Allors
        [Id("145BFCA4-4B4C-4E35-A0D1-4DAB74481ABE")]
        [AssociationId("3AB3AC09-75B5-440D-870D-9458E1907978")]
        [RoleId("F1B06A6D-3286-4222-8906-61648630DBAF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Indexed]
        SalesTerm[] SalesTerms { get; set; }

        #region Allors
        [Id("CBC89E70-0E91-4CD2-ABCA-C3C2E998339C")]
        [AssociationId("74AF9EAF-F2E3-4BCB-B7AC-993A78DD1AC4")]
        [RoleId("341B2C8E-D22D-447C-BBCB-CE52297CD554")]
        #endregion
        [Workspace]
        [Size(-1)]
        string ShippingInstruction { get; set; }

        #region Allors
        [Id("1C36B7F3-4C5C-4BB8-91C8-4EE7B9468931")]
        [AssociationId("0D3BFCF8-EAA0-42F1-B3DC-DA3815A66CB0")]
        [RoleId("9AE1DC62-B23D-48BF-8B8E-07156A1E22A1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Indexed]
        OrderItem[] Associations { get; set; }

        #region Allors
        [Id("b16be678-3468-447c-b971-34f519820972")]
        [AssociationId("ae214eec-b760-4b74-9c75-7773a92740d6")]
        [RoleId("5031231d-8f6e-48b8-8982-4bc7d821a1df")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime DerivedIrpfRegime { get; set; }

        #region Allors
        [Id("e62b5afb-21a3-4ab3-b1c8-77a90a605cd3")]
        [AssociationId("cfee2c61-98b6-4233-9ef3-e391e56480d2")]
        [RoleId("2af9c4bd-1e6a-4a30-98f7-0ed7e8980484")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("e51e9a39-872a-469f-87c7-41e5efc206b3")]
        [AssociationId("33a32a1e-d75b-434f-9682-f10f4eb767cd")]
        [RoleId("333bd1d1-ec24-4c5c-a92a-657a79f1a5d5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRate IrpfRate { get; set; }

        #region Allors
        [Id("d343ff1f-05b5-4d8a-bd62-c89feccb9784")]
        [AssociationId("a7b03865-9c43-4ca3-9f9f-15c09dc2c278")]
        [RoleId("bafbdbbd-316e-4653-9fbc-e965b436bc12")]
        #endregion
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal UnitIrpf { get; set; }

        #region Allors
        [Id("cd93af05-5f00-415b-9e6a-d53e370b202e")]
        [AssociationId("b900e3e1-779b-4e00-9dc9-8fbcb1f07c69")]
        [RoleId("78f666ea-c0ea-4ee0-9ded-18ee5af4c26e")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpf { get; set; }

        #region Allors
        [Id("E5664F18-C9AA-4590-9882-4DD7FAF3C187")]
        [AssociationId("E547A284-85C4-4A63-B51A-75BC50AAF139")]
        [RoleId("A36683BD-9F49-45BF-B4D2-D4634AF85EB9")]
        #endregion
        [Workspace]
        [Size(-1)]
        string Message { get; set; }
    }
}
