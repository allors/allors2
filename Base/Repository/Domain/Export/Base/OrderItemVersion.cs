namespace Allors.Repository
{
    using System;

    using Attributes;

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
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal PreviousQuantity { get; set; }

        #region Allors
        [Id("EDD60449-DBC2-4102-BF83-48CE84C528A8")]
        [AssociationId("2A188225-B9EB-4918-ADCA-F90721A5DB8A")]
        [RoleId("F8B52F60-15C2-4858-A1F0-28C0CC42D7DB")]
        #endregion
        [Workspace]
        [Required]
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
        [Derived]
        [Required]
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
        [Derived]
        [Workspace]
        DateTime DeliveryDate { get; set; }

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
        [Id("E5664F18-C9AA-4590-9882-4DD7FAF3C187")]
        [AssociationId("E547A284-85C4-4A63-B51A-75BC50AAF139")]
        [RoleId("A36683BD-9F49-45BF-B4D2-D4634AF85EB9")]
        #endregion
        [Workspace]
        [Size(-1)]
        string Message { get; set; }
    }
}
