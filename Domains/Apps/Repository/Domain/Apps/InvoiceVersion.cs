namespace Allors.Repository
{
    using System;

    using Attributes;

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
        [Id("61B6F5CB-ABB0-47F9-85A4-6F22ADD9868F")]
        [AssociationId("2986D2FD-3DD2-476D-8281-304EB911D97E")]
        [RoleId("A3F32A6C-E4AB-49B4-95A2-81222A845A9B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User LastModifiedBy { get; set; }

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
        [Id("A3F9991E-2988-469A-AA75-935C648386C5")]
        [AssociationId("FC354A2B-0EE8-4845-8493-8E35A5142AFD")]
        [RoleId("987620F8-D73C-4C2E-9947-046A1D5E3981")]
        #endregion
        [Workspace]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandlingCustomerCurrency { get; set; }

        #region Allors
        [Id("2B43A67D-2E2A-4D10-B4A7-024C85B5FC74")]
        [AssociationId("FC35C74E-471C-4FBD-98F9-A303B812AA23")]
        [RoleId("34225276-6C0C-4D34-A158-00420DACD434")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Derived]
        [Indexed]
        Currency CustomerCurrency { get; set; }

        #region Allors
        [Id("6F65890E-B409-4A70-887A-778C6703AEFB")]
        [AssociationId("95E25BDE-EB9A-4F15-8092-2ED4CE05F58C")]
        [RoleId("41CF0543-FE36-44A4-ACF1-8277D08274E3")]
        #endregion
        [Size(256)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("534B8467-D56C-4720-95AB-1A8CBDF0CA4B")]
        [AssociationId("1005EFFB-29E1-44D8-9070-07B180A9EBE3")]
        [RoleId("6446D5E8-BDB8-482C-AA60-14DD45F377B5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }

        #region Allors
        [Id("CA1E5F85-2FAC-4D1F-A6EC-8B79A02B450E")]
        [AssociationId("58D8573D-A7D1-4162-B137-F237BDE6124C")]
        [RoleId("16CC8C9A-1BF7-4A5C-BC46-E734AF15FE4A")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalFeeCustomerCurrency { get; set; }

        #region Allors
        [Id("241A6D98-7DD0-4724-B891-56663970A99C")]
        [AssociationId("3C1276B4-F027-43D9-8B2A-284B9EBFA584")]
        [RoleId("3A6B01CF-35D7-42F8-9B0A-74E9B6E67A36")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Fee Fee { get; set; }

        #region Allors
        [Id("CB76DE8D-257D-4746-B81B-4F3716202603")]
        [AssociationId("109B6D2E-353C-4310-AF36-7F23DEB46145")]
        [RoleId("6F1F6004-1335-4A0B-9BF9-E6B27F735204")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExVatCustomerCurrency { get; set; }

        #region Allors
        [Id("56151707-E3E4-4CE0-AB6A-63C64DD42DC6")]
        [AssociationId("1D735AD7-491F-412F-BECB-D9385357FF3D")]
        [RoleId("FF2EB983-450D-443C-8D34-4D28A2AEFB71")]
        #endregion
        [Size(256)]
        [Workspace]
        string CustomerReference { get; set; }

        #region Allors
        [Id("69476B4F-CD64-4ADC-98E3-2E8CF9C5DADA")]
        [AssociationId("815DC3A4-72CA-4CEC-BCA1-122F579AC5B5")]
        [RoleId("27603BEE-E380-4D60-A082-B430316F1AB6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        DiscountAdjustment DiscountAdjustment { get; set; }

        #region Allors
        [Id("6C96867C-D07F-4AB4-BD7D-5E622A5B55BE")]
        [AssociationId("1E852B92-0AA6-4AD8-85E0-73A6C70DD4AA")]
        [RoleId("4F49FCB5-46A2-434A-9A91-AB5500365DCE")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountPaid { get; set; }

        #region Allors
        [Id("CCFACA15-C8BC-4260-AA9E-0B4739281590")]
        [AssociationId("4CDD5CE8-8825-421E-A5DB-1D6A0E2F26CD")]
        [RoleId("6ADC6E83-0FA4-4F66-AE3F-96DEF30979FE")]
        #endregion
        [Derived]
        [Required]
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
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("D17019EC-A2AB-4063-B862-EF95A867CE61")]
        [AssociationId("04DA9D01-91A1-403B-988F-43BA2DF9FB16")]
        [RoleId("043FDAFA-32BE-42B9-B9C5-38DA573AB680")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("3D03D4F2-E82C-4802-BABC-ADD692925C44")]
        [AssociationId("996AFC47-7239-488B-A558-6E67BD4BC38C")]
        [RoleId("82B5CD63-FC62-4C1C-9FB6-046C8731B333")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("D6B72898-4755-4800-80C6-26913FB48795")]
        [AssociationId("EC1D96C5-5473-4738-95CA-4A29CF80F0EC")]
        [RoleId("85047F02-0827-4C31-9147-517C621C4380")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalVatCustomerCurrency { get; set; }

        #region Allors
        [Id("4F0BDB92-1D82-4489-AF4B-35000DC8EA6A")]
        [AssociationId("684D2BBF-CACB-4256-AE82-CBA65A03D054")]
        [RoleId("EC9CDE94-354A-4850-A10B-2B0845CB567C")]
        #endregion
        [Required]
        [Workspace]
        DateTime InvoiceDate { get; set; }

        #region Allors
        [Id("F4B96308-2791-4A0B-A83D-462D22141968")]
        [AssociationId("683FCC67-EA7F-4E1A-8C88-8914E761A9B4")]
        [RoleId("AC3E7EFC-6FB8-45B5-8D6B-67618E522591")]
        #endregion
        [Derived]
        [Required]
        [Workspace]
        DateTime EntryDate { get; set; }

        #region Allors
        [Id("4AFC0E6B-6B28-4BAD-A47C-6530D6F9D81B")]
        [AssociationId("7E383CFC-6E40-4350-8FD5-60031BE8967D")]
        [RoleId("248563C1-D4A7-470A-A0D8-1138921419EA")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIncVatCustomerCurrency { get; set; }

        #region Allors
        [Id("D5A20009-50CC-4605-AAA1-554C6B478931")]
        [AssociationId("701CE24A-A0B6-4503-B773-615F083AC733")]
        [RoleId("6FEAB01B-F73F-4336-A62E-5ECA563F9A50")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("469C0A74-7513-42A6-8B39-757C81DC08F6")]
        [AssociationId("EA357FFE-1DA5-418B-BED3-17F527A41893")]
        [RoleId("63C03128-26CE-4235-BB4B-2487675A97A4")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalBasePriceCustomerCurrency { get; set; }


        #region Allors
        [Id("ABA0E587-8A3C-446A-A3E2-D79DDC67D1CC")]
        [AssociationId("30DA74A5-8F28-4266-9182-E239AAB422AD")]
        [RoleId("DEAFC9A3-D5F4-4A6E-B727-BECF2ECD9D73")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        SurchargeAdjustment SurchargeAdjustment { get; set; }

        #region Allors
        [Id("2B3F165E-27C4-461B-9A0B-6107CEC37200")]
        [AssociationId("E7809A02-A053-4C6F-9734-B7730A7BF965")]
        [RoleId("76DE0F8E-339B-4A93-8254-15D641180F3D")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExVat { get; set; }

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
        [Id("6771A180-CC25-4B33-92FE-3204456AF9BA")]
        [AssociationId("936431AF-BF68-4548-AC4A-A6D7FE298B88")]
        [RoleId("03D8EFA6-C67A-415F-824A-131F64B2EA1E")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalSurchargeCustomerCurrency { get; set; }

        #region Allors
        [Id("05D98EF9-5464-4C37-A833-47B76DA57F24")]
        [AssociationId("B74F0010-BFA5-4B5D-8C3D-589C899BE378")]
        [RoleId("E43CA923-899B-4CDF-BA96-5A69A795BDD1")]
        #endregion
        [Required]
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
        VatRegime VatRegime { get; set; }

        #region Allors
        [Id("4100FCC9-7660-4B86-BBE3-A28B3955F687")]
        [AssociationId("B033B6B0-E29C-47C1-A94B-EF96B472502B")]
        [RoleId("431B3265-0C90-4DA8-B413-5616E5461FAF")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalDiscountCustomerCurrency { get; set; }

        #region Allors
        [Id("070DDB3E-F6F6-42C1-B723-4D8C406BD2E8")]
        [AssociationId("13275A3E-C5ED-4A21-8854-998FB57414F0")]
        [RoleId("93A2735C-0C71-40D5-A1C2-392141C2367F")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("7E572C4E-C2B5-4008-93CC-D9F909EAF0C6")]
        [AssociationId("B1CF18D5-623E-47F8-A518-385425D32144")]
        [RoleId("82BE11EF-4994-4F76-997E-4779C72690C8")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalFee { get; set; }
    }
}