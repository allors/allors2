// <copyright file="InvoiceVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("683DF770-70BE-4E31-830A-B9DD9031030D")]
    #endregion
    public partial interface InvoiceVersion : Version
    {
        #region Allors
        [Id("3C21D07A-3723-4D0C-BCDF-2A699DB6794F")]
        [AssociationId("2DF9BA58-5405-4205-8368-4C4F26437A22")]
        [RoleId("42E7DA43-55F7-44F4-9FED-82EA966BBADB")]
        [Indexed]
        #endregion
        [Size(-1)]
        [Workspace]
        string Comment { get; set; }

        #region Allors
        [Id("85356434-312C-4147-8D32-D27BE1B103B6")]
        [AssociationId("0C448664-2CAC-4899-9F19-BA88AEE229E7")]
        [RoleId("39F59849-515F-45F4-8668-237D8907E513")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User CreatedBy { get; set; }

        #region Allors
        [Id("1E73052D-5404-4D10-B353-493906CED780")]
        [AssociationId("0704502C-5A65-4E44-9743-3879DD25207E")]
        [RoleId("3B4F3C0D-B711-4BB7-8C79-BA99388645B9")]
        #endregion
        [Workspace]
        DateTime CreationDate { get; set; }

        #region Allors
        [Id("93C86CD1-0329-4303-8EC4-DAAB6BF451C1")]
        [AssociationId("FADD905C-1682-4307-8DD0-9B231279A37C")]
        [RoleId("CCBD503A-9424-4571-A266-D8F54DDADC67")]
        #endregion
        [Workspace]
        DateTime LastModifiedDate { get; set; }

        #region Allors
        [Id("3A694B6A-A891-418C-9A7C-3709978B7761")]
        [AssociationId("C1422EA3-1FD4-4082-9768-CBBD955062C5")]
        [RoleId("6C9A4674-0ACC-4279-B23F-7F20AFA50391")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("2B43A67D-2E2A-4D10-B4A7-024C85B5FC74")]
        [AssociationId("FC35C74E-471C-4FBD-98F9-A303B812AA23")]
        [RoleId("34225276-6C0C-4D34-A158-00420DACD434")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        Currency AssignedCurrency { get; set; }

        #region Allors
        [Id("6F65890E-B409-4A70-887A-778C6703AEFB")]
        [AssociationId("95E25BDE-EB9A-4F15-8092-2ED4CE05F58C")]
        [RoleId("41CF0543-FE36-44A4-ACF1-8277D08274E3")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("56151707-E3E4-4CE0-AB6A-63C64DD42DC6")]
        [AssociationId("1D735AD7-491F-412F-BECB-D9385357FF3D")]
        [RoleId("FF2EB983-450D-443C-8D34-4D28A2AEFB71")]
        #endregion
        [Workspace]
        string CustomerReference { get; set; }

        #region Allors
        [Id("6C96867C-D07F-4AB4-BD7D-5E622A5B55BE")]
        [AssociationId("1E852B92-0AA6-4AD8-85E0-73A6C70DD4AA")]
        [RoleId("4F49FCB5-46A2-434A-9A91-AB5500365DCE")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountPaid { get; set; }

        #region Allors
        [Id("CCFACA15-C8BC-4260-AA9E-0B4739281590")]
        [AssociationId("4CDD5CE8-8825-421E-A5DB-1D6A0E2F26CD")]
        [RoleId("6ADC6E83-0FA4-4F66-AE3F-96DEF30979FE")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("49FA267E-4801-4347-852C-BFA556FDB4B5")]
        [AssociationId("78991711-4EE5-4DD7-AF14-5915B8B0585D")]
        [RoleId("E93F4651-8D75-49B7-ACE1-0D93A5C6F702")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        BillingAccount BillingAccount { get; set; }

        #region Allors
        [Id("5913C137-F2C7-4C77-B005-2935853CE2A8")]
        [AssociationId("4409ED13-4614-4069-ACBF-5ED52DC53F77")]
        [RoleId("9E266B19-DE3E-4FC6-9B7A-E999139A8A3E")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("D17019EC-A2AB-4063-B862-EF95A867CE61")]
        [AssociationId("04DA9D01-91A1-403B-988F-43BA2DF9FB16")]
        [RoleId("043FDAFA-32BE-42B9-B9C5-38DA573AB680")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("3D03D4F2-E82C-4802-BABC-ADD692925C44")]
        [AssociationId("996AFC47-7239-488B-A558-6E67BD4BC38C")]
        [RoleId("82B5CD63-FC62-4C1C-9FB6-046C8731B333")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("d60855bf-af6c-4871-927f-9e1ac689979a")]
        [AssociationId("203ffebd-4cd6-40ad-96c1-d1e46b2b91f3")]
        [RoleId("87ad1baa-4dd1-494d-8eab-8bf6319fa18c")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotal { get; set; }

        #region Allors
        [Id("4F0BDB92-1D82-4489-AF4B-35000DC8EA6A")]
        [AssociationId("684D2BBF-CACB-4256-AE82-CBA65A03D054")]
        [RoleId("EC9CDE94-354A-4850-A10B-2B0845CB567C")]
        #endregion
        [Workspace]
        DateTime InvoiceDate { get; set; }

        #region Allors
        [Id("F4B96308-2791-4A0B-A83D-462D22141968")]
        [AssociationId("683FCC67-EA7F-4E1A-8C88-8914E761A9B4")]
        [RoleId("AC3E7EFC-6FB8-45B5-8D6B-67618E522591")]
        #endregion
        [Workspace]
        DateTime EntryDate { get; set; }

        #region Allors
        [Id("D5A20009-50CC-4605-AAA1-554C6B478931")]
        [AssociationId("701CE24A-A0B6-4503-B773-615F083AC733")]
        [RoleId("6FEAB01B-F73F-4336-A62E-5ECA563F9A50")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("2B3F165E-27C4-461B-9A0B-6107CEC37200")]
        [AssociationId("E7809A02-A053-4C6F-9734-B7730A7BF965")]
        [RoleId("76DE0F8E-339B-4A93-8254-15D641180F3D")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("1a9136c4-1d51-4db7-a434-97e4d87bdba1")]
        [AssociationId("54118780-6f82-4d58-9c99-9ec2a5419bc5")]
        [RoleId("08326ef0-2802-488e-9d12-67c4ee7e13a7")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        OrderAdjustment[] OrderAdjustments { get; set; }

        #region Allors
        [Id("E6F77579-FB47-42B1-8A0C-D699A491AA18")]
        [AssociationId("4E76262F-31BB-42D2-9406-7E3BFC118609")]
        [RoleId("C6D16AC9-A816-469D-BBB8-C864FE556E3A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        SalesTerm[] SalesTerms { get; set; }

        #region Allors
        [Id("05D98EF9-5464-4C37-A833-47B76DA57F24")]
        [AssociationId("B74F0010-BFA5-4B5D-8C3D-589C899BE378")]
        [RoleId("E43CA923-899B-4CDF-BA96-5A69A795BDD1")]
        #endregion
        [Size(256)]
        [Workspace]
        string InvoiceNumber { get; set; }

        #region Allors
        [Id("CCCAFE62-A111-4000-852C-1621B5B009EA")]
        [AssociationId("13D86579-A0A5-46C5-BC95-25EA54F985CC")]
        [RoleId("F09B1D69-462F-4A0C-A0F5-2F7D1F10EBB4")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Message { get; set; }

        #region Allors
        [Id("141D8BBF-5EE1-4521-BFBC-34D9F69BEADA")]
        [AssociationId("EA626F7E-A89E-4BA4-A5AE-A01FF4514567")]
        [RoleId("65F299CF-5D23-4C72-95AB-548F20437364")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        VatRegime AssignedVatRegime { get; set; }

        #region Allors
        [Id("850af264-a601-4070-ae75-1c608aacb0dc")]
        [AssociationId("b5172607-a7f5-41f8-aabc-d0e376369781")]
        [RoleId("a8b85fbf-54e7-4f37-a8e8-b026700d374e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        VatRegime DerivedVatRegime { get; set; }

        #region Allors
        [Id("613afd54-e378-4998-ba2d-777f691c0cf7")]
        [AssociationId("78ebb5ae-c1a3-4734-b0e0-339c2d3c5c11")]
        [RoleId("dfb8b271-1a14-41d8-b50b-863b3bcf0fa7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("c7f5396d-026c-4036-916c-c3b91b7fa288")]
        [AssociationId("0190d571-9dc2-43f1-87fb-79c35e49d3fd")]
        [RoleId("15845da0-c31e-418b-839b-8642619a0732")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime DerivedIrpfRegime { get; set; }

        #region Allors
        [Id("070DDB3E-F6F6-42C1-B723-4D8C406BD2E8")]
        [AssociationId("13275A3E-C5ED-4A21-8854-998FB57414F0")]
        [RoleId("93A2735C-0C71-40D5-A1C2-392141C2367F")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("7E572C4E-C2B5-4008-93CC-D9F909EAF0C6")]
        [AssociationId("B1CF18D5-623E-47F8-A518-385425D32144")]
        [RoleId("82BE11EF-4994-4F76-997E-4779C72690C8")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalFee { get; set; }

        #region Allors
        [Id("e1528797-7ff6-41cc-aa0e-b043d7a1f1c4")]
        [AssociationId("043f1aae-660e-4dac-b1ff-74c75cbfa357")]
        [RoleId("1163b076-01a1-4758-a2f7-c4c4a37a1431")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraCharge { get; set; }

        #region Allors
        [Id("862aef24-b641-4425-830e-04fd811373d7")]
        [AssociationId("eb7065d2-48a9-459b-a6fe-a10048df45c2")]
        [RoleId("0b54c320-41e8-4440-b545-9fd9a3393525")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpfInPreferredCurrency { get; set; }

        #region Allors
        [Id("91dadffb-0ba9-43bb-9ba5-1f5b25b50018")]
        [AssociationId("e85ac811-ff89-46cb-8cf2-a2e761df94d8")]
        [RoleId("cfbcc1ad-20c2-4052-b064-8306c77ae9e2")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("5a1ee643-f4b1-439f-bdde-1c8248605776")]
        [AssociationId("46c24f24-f373-4432-8ddf-aa1b4508f4ed")]
        [RoleId("d44118a1-9067-4124-9ce1-0f0db4eba92f")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("23353331-a3d7-47fd-948c-47a8db4e098c")]
        [AssociationId("56ffca17-babd-4faa-87cc-c122db1bcfa5")]
        [RoleId("403a51b1-c13a-4351-8238-9278bfe6abbd")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("1a88e51f-5d7c-401d-b5c2-ca743cdd5a28")]
        [AssociationId("9a7b194e-d2b5-43d8-b389-58d614f04c90")]
        [RoleId("e2ba2dd8-cf6e-4222-b812-96fa8ca6bcd3")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurchargeInPreferredCurrency { get; set; }

        #region Allors
        [Id("7edd9403-628e-4f41-bb2f-eb8deef4f852")]
        [AssociationId("ebf97891-a8f1-4d41-a71b-6b1243bdee90")]
        [RoleId("82761f13-a776-4038-ad6e-8f7b46df27b2")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscountInPreferredCurrency { get; set; }

        #region Allors
        [Id("c7805ce6-a43c-4066-9f66-c6b3cd7aed18")]
        [AssociationId("5b9996e1-fe20-49bb-b37a-44f9947551c5")]
        [RoleId("abdbea6c-7fc9-4045-bc88-68b287790433")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandlingInPreferredCurrency { get; set; }

        #region Allors
        [Id("1948e9db-05aa-42f0-a6c1-7ac31a7616f5")]
        [AssociationId("f8a8edf2-e020-44e1-b008-7e18422373db")]
        [RoleId("60297674-8a00-4763-9ece-2a48b85f357b")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFeeInPreferredCurrency { get; set; }

        #region Allors
        [Id("642ff657-fffb-45d3-a55e-a57ab14ca5ad")]
        [AssociationId("cfaf5838-6169-4d60-af71-b1c91c256fa3")]
        [RoleId("b4a110a5-d438-4898-a809-ea39a950f615")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraChargeInPreferredCurrency { get; set; }

        #region Allors
        [Id("c066b0b9-df6b-423d-91b4-7f070cbce3f9")]
        [AssociationId("4521ce7e-f4b9-470f-b823-536ba1e16434")]
        [RoleId("624ae960-fd8e-4319-a027-ee468c5d211b")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePriceInPreferredCurrency { get; set; }

        #region Allors
        [Id("0f23fc4b-a49d-4169-b3a2-c1c43e66c0c0")]
        [AssociationId("715a1d32-ca17-4d75-8be7-84860db99921")]
        [RoleId("392de37e-82ca-48ea-bc11-3663be30d44a")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPriceInPreferredCurrency { get; set; }

        #region Allors
        [Id("66bbd222-3700-48f9-aa74-3f292d9ca1f0")]
        [AssociationId("9bacc03f-5994-4a2a-982f-9f4588348554")]
        [RoleId("7f155aa9-09df-4322-8d1e-dd4cf96946d0")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotalInPreferredCurrency { get; set; }
    }
}
