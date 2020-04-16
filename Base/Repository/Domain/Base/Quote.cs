// <copyright file="Quote.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("066bf242-2710-4a68-8ff6-ce4d7d88a04a")]
    #endregion
    public partial interface Quote : Transitional, WorkItem, Printable, Auditable, Commentable
    {
        #region ObjectStates
        #region QuoteState
        #region Allors
        [Id("B1792FCE-33EF-4A03-BCB7-92E839A55B2C")]
        [AssociationId("2FCD7B16-863B-4A41-9C2C-B7E45E74799A")]
        [RoleId("3E5206B4-40F2-4939-BA95-2D8D089CFDF5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        QuoteState PreviousQuoteState { get; set; }

        #region Allors
        [Id("C1B9AD76-9773-4A52-AADB-ED3E7222C89B")]
        [AssociationId("9945B2C0-06CC-4410-8861-05A54DDB8728")]
        [RoleId("9BEA275E-F1A0-4023-877E-EF67A150C3DF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        QuoteState LastQuoteState { get; set; }

        #region Allors
        [Id("2A4AADE6-B3F0-436B-BA9E-5D0ECB958077")]
        [AssociationId("62A27DA3-372A-4296-B7C6-0BD482A7CB31")]
        [RoleId("84F136C7-B146-4D20-A4D2-797740E60291")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        QuoteState QuoteState { get; set; }
        #endregion
        #endregion

        #region Allors
        [Id("AFB30FBE-9E93-4EBD-B8D3-5C5B231D70E1")]
        [AssociationId("520CF705-3E9C-4845-816C-F8DD17A4534B")]
        [RoleId("7B78FA1B-EAEA-419D-87E8-F0A746DBEA18")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        InternalOrganisation Issuer { get; set; }

        #region Allors
        [Id("3B913CC6-C627-4F16-ACF5-98EC97CE5FDA")]
        [AssociationId("7CD50B18-3C4A-4A81-B6AA-9CE8BC43C0DA")]
        [RoleId("3A53DEDC-0877-44FA-89E3-1212EAB4FF36")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("D566DD5B-BF58-45A6-A68F-FD7D2652FB4D")]
        [AssociationId("8F0A31C1-1B2A-4531-84BA-7636F4D0B9DF")]
        [RoleId("00EF6AB3-E075-45F5-A839-96576E3546AB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        DateTime RequiredResponseDate { get; set; }

        #region Allors
        [Id("b5ecffab-0f27-4311-9f66-197f0cdc147f")]
        [AssociationId("37904354-0db4-43b2-8885-4e9e57b915f4")]
        [RoleId("f57f743f-5c90-4348-b9a0-52dc6b2116ae")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        QuoteItem[] ValidQuoteItems { get; set; }

        #region Allors
        [Id("033df6dd-fdf7-44e4-84ca-5c7e100cb3f5")]
        [AssociationId("4b19f443-0d27-447d-8186-e5361a094460")]
        [RoleId("fa17ef86-c074-414e-b223-b62522d68280")]
        #endregion
        [Workspace]
        DateTime ValidFromDate { get; set; }

        #region Allors
        [Id("05e3454a-0a7a-488d-b4b1-f0fd41392ddf")]
        [AssociationId("ca3f0d26-9ead-4691-8f7f-f79272065251")]
        [RoleId("92e46228-ad44-4b9b-b727-23159a59bca3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        QuoteTerm[] QuoteTerms { get; set; }

        #region Allors
        [Id("2140e106-2ef3-427a-be94-458c2b8e154d")]
        [AssociationId("9d81ada4-a4f3-44bb-9098-bc1a3e61de19")]
        [RoleId("60581583-2536-4b09-acae-f0f877169dae")]
        #endregion
        [Workspace]
        DateTime ValidThroughDate { get; set; }

        #region Allors
        [Id("3da51ccc-24b9-4b03-9218-7da06492224d")]
        [AssociationId("602c70c9-ddc4-4cf5-a79f-0abcc0beba15")]
        [RoleId("d4d93ad0-c59d-40e7-a82c-4fb1e54a85f2")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("9119c598-cd98-43da-bfdf-1e6573112c9e")]
        [AssociationId("d48cd46d-889b-4e2d-a6d6-ee26f30fb3e5")]
        [RoleId("56f5d5ee-1ab5-48f2-a413-7b80dd2c283e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        Party Receiver { get; set; }

        #region Allors
        [Id("A1C248DF-7F2A-4622-9052-9106C67B1D71")]
        [AssociationId("ED908492-D8C8-40E8-853D-6F9602B1D646")]
        [RoleId("69D3F384-64C8-4D6C-AEA7-CCDA7CB51DCF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        ContactMechanism FullfillContactMechanism { get; set; }

        #region Allors
        [Id("37D046B8-3804-4912-9B53-C98D66A67BC0")]
        [AssociationId("0608C530-D264-46A8-B298-367CCC0FEF76")]
        [RoleId("F7A1DEFC-271D-43BD-B05F-2D9441C7EE3A")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        VatRegime VatRegime { get; set; }

        #region Allors
        [Id("0B00B80D-1A5C-4CB0-A50A-B6E552A1AF6F")]
        [AssociationId("FC12589D-86F4-40A9-BBA6-8F0A21F42BC7")]
        [RoleId("C550C3E6-22EA-41B4-9F11-E45CC4BFBFE9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        VatClause AssignedVatClause { get; set; }

        #region Allors
        [Id("3B47127B-7C65-4891-8D24-4622D7573EC5")]
        [AssociationId("6C19844B-6CB5-4260-93D3-EBFD9C3FAB28")]
        [RoleId("B65F6838-2DF5-4EA8-AF99-5B983216B76C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        VatClause DerivedVatClause { get; set; }

        #region Allors
        [Id("E38FDC05-A2BB-4E37-9B92-7976AEB5AD4E")]
        [AssociationId("CA767808-44B0-45ED-9B6E-23D671BA0A3B")]
        [RoleId("EBD390F4-6BC0-4497-A5E3-E67C46055CE2")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("2BB55996-6992-4960-ADD3-7B2AB846DBC3")]
        [AssociationId("7EB76ED7-4D4F-4AE0-9D1C-B7B5B6A8809B")]
        [RoleId("107337AA-EC3B-4A53-BDBE-CC9156C81D66")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("2163DB4B-684D-45BF-B56A-99D5311DDC52")]
        [AssociationId("C0A4674C-7860-4C64-81D2-49C930D04B6C")]
        [RoleId("5D293D35-7700-4DE0-9905-C5E309B689DE")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("DB9E318C-FE6E-4A84-89DA-AD02EE9C3266")]
        [AssociationId("A35B6FE6-E152-43A2-9F81-944C870C115D")]
        [RoleId("69245985-8496-4703-9467-511974A69834")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("81140E8D-B94B-47AD-B478-E3AD91C1E66E")]
        [AssociationId("1192E38B-0350-42FB-A48D-098A79004AD4")]
        [RoleId("836B6F9C-BAC3-42FD-8F46-8CAFEAEB0EF5")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("3FD4A223-4EDB-44B7-8AE4-CC9B0AA9FEEE")]
        [AssociationId("379DDEBA-50D1-4F55-9CF5-C1FDC4E1D25F")]
        [RoleId("D396C4CB-3CDF-492D-8B3A-E26DADA94EAB")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("7B8FB84F-02FD-4616-BD09-2D7E421FBB5B")]
        [AssociationId("2BFBD0D1-83B9-4784-B2A3-D82938CF0A57")]
        [RoleId("D5373E0F-23EC-4E1B-8ED2-BC945B14FE84")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFee { get; set; }

        #region Allors
        [Id("B02CBBD4-0D39-4851-80FA-E0727CF38353")]
        [AssociationId("E04679D7-7DA8-4073-AAC5-F6B17E8FCD47")]
        [RoleId("E3802964-CC33-474E-AC9C-F7950EE36EF7")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("706BEBDC-C2E3-4534-B94C-B6A8D3222BA1")]
        [AssociationId("A0748124-471E-4277-B802-B63DA32559AC")]
        [RoleId("4902C7D6-58C2-40C6-BF3C-7ABD926C9269")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPrice { get; set; }

        #region Allors
        [Id("C8809848-28D4-40E8-97A0-3663C4AC9745")]
        [AssociationId("6CE85935-4B54-4422-B052-BFBA71D10C3A")]
        [RoleId("60D0AA32-286F-41C5-ADED-691A953C9445")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }

        #region Allors
        [Id("BB1DEC96-411E-45F3-AB9A-888B978E1F3E")]
        [AssociationId("1641A308-7405-4606-A9DF-1A9E3FAF49C3")]
        [RoleId("692FBF8B-9D5A-4420-84A9-66F66AED0913")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        DiscountAdjustment DiscountAdjustment { get; set; }

        #region Allors
        [Id("1F0A646A-2B24-408E-A457-AABAE527BA95")]
        [AssociationId("E91BED60-ADE5-4175-936D-912DA9D84907")]
        [RoleId("52CA6C14-B572-42CA-B3A7-8191C720986D")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        SurchargeAdjustment SurchargeAdjustment { get; set; }

        #region Allors
        [Id("D2B0F48B-70B5-4098-A85E-F574B9328213")]
        [AssociationId("3ABF21A1-7C8B-4036-B2E5-D828DCA83B33")]
        [RoleId("22217C90-F4C1-444A-A869-F626FD915527")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        Fee Fee { get; set; }

        #region Allors
        [Id("BA16DE57-19A1-40BC-AF3C-99690EB5ECAB")]
        [AssociationId("B1D552EE-4967-4651-95C9-C9A9A846DE35")]
        [RoleId("E6732FB3-5BD9-4AB1-B5B9-394D643A47E0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Currency Currency { get; set; }

        #region Allors
        [Id("d7dc81e8-76e7-4c68-9843-a2aaf8293510")]
        [AssociationId("6fbc80d1-e72b-4484-a9b1-e606f15d2435")]
        [RoleId("219cb27f-20b5-48b3-9d89-4b119798b092")]
        #endregion
        [Workspace]
        [Required]
        DateTime IssueDate { get; set; }

        #region Allors
        [Id("e250154a-77c5-4a0b-ae3d-28668a9037d1")]
        [AssociationId("b5ba8cfd-2b16-4a50-89cd-46927d59b97a")]
        [RoleId("f5b6881b-c4d5-42e3-a024-0ae4564cb970")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        QuoteItem[] QuoteItems { get; set; }

        #region Allors
        [Id("e76cbd73-78b7-4ef8-a24c-9ac0db152f7f")]
        [AssociationId("057ad29f-c245-44b2-8a95-71bd6607830b")]
        [RoleId("218e3a6e-b530-41f7-a60e-7587f8072c8c")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string QuoteNumber { get; set; }

        #region Allors
        [Id("94DE208B-5FF9-45F5-BD35-5BB7D7B33FB7")]
        [AssociationId("10F24038-D3E1-40DC-9D4F-27E738BE7F3D")]
        [RoleId("3D269537-523B-489A-A7DA-008DC8585F60")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        Request Request { get; set; }

        #region Allors
        [Id("2D6804B9-A745-497A-9F43-FADE6B1B76AB")]
        [AssociationId("542367E1-1CAB-4DB6-9FB3-ECAD8930601F")]
        [RoleId("EDF15A48-DF46-411C-B73A-9B6E9C184932")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Person ContactPerson { get; set; }

        #region Allors

        [Id("8C858157-B9BC-4E2C-97BC-646066532854")]

        #endregion
        [Workspace]
        void Create();

        #region Allors

        [Id("70F1138B-1383-4AA1-A08E-6C99F71F3F07")]

        #endregion
        [Workspace]
        void Reopen();

        #region Allors
        [Id("519F70DC-0C4C-43E7-8929-378D8871CD84")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("506ED1BA-5F88-487E-B126-470FE1FD7791")]
        #endregion
        [Workspace]
        void Send();

        #region Allors
        [Id("90c9c005-3842-44f5-877c-f601523a888f")]
        #endregion
        [Workspace]
        void Accept();

        #region Allors
        [Id("6b5e540d-96a8-48cf-a888-7e7f6b844d28")]
        #endregion
        [Workspace]
        void Revise();

        #region Allors
        [Id("39694549-7173-4904-8AE0-DA7390F595A5")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("712D9F73-0D39-4F25-9CD3-6D8BE6F8AEC8")]
        #endregion
        [Workspace]
        void Cancel();
    }
}
