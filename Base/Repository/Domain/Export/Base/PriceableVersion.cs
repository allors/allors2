namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("7CFD2B40-4C45-489B-9F92-83E7E1641F19")]
    #endregion
    public partial interface PriceableVersion : Version
    {
        #region Allors
        [Id("A2B92C8C-3263-47AE-9345-16A04A4A5AEC")]
        [AssociationId("3703743C-8241-4B53-8F96-82A1BD9FE1F9")]
        [RoleId("8F9AF702-9B6A-478B-973B-1B6C52650665")]
        #endregion
        [Derived]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscountAsPercentage { get; set; }

        #region Allors
        [Id("97F0156D-82B8-40F6-A4E4-F0F975C1C57F")]
        [AssociationId("8C825CC6-AECD-44DE-9DAC-BE1D29FB7660")]
        [RoleId("98566D81-9BF6-48AC-AA78-7C21D55A5F41")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        DiscountAdjustment DiscountAdjustment { get; set; }

        #region Allors
        [Id("DBB03B6B-93EA-41FD-838D-D89C38D192FA")]
        [AssociationId("C33FB42C-A297-4F9A-8208-E5CD733806B6")]
        [RoleId("CDC5EB9F-3CB8-4322-931B-958A00A72217")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal UnitVat { get; set; }

        #region Allors
        [Id("C087BCE3-6075-48FC-9D71-5EC4B8956E21")]
        [AssociationId("E1E2DC3C-403A-4447-9D12-5A186C14035C")]
        [RoleId("7F8FE276-65BE-4FB1-9FEC-A57A5F0CDEB0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        VatRegime VatRegime { get; set; }

        #region Allors
        [Id("D05C5D37-1915-445C-AC89-0CF1E0D1636E")]
        [AssociationId("103EA1E0-8349-4AB8-9C03-3C6736860F89")]
        [RoleId("C0122B05-24A8-44E5-B03E-F994B0F3D107")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("949C917B-718B-44E9-8E00-C620ECDDB003")]
        [AssociationId("6A317B49-54A9-4338-930A-5CDFDDE2CFC6")]
        [RoleId("AEA6D92E-9E1A-4B3A-BC9C-AF4EC4FD6D38")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal UnitSurcharge { get; set; }

        #region Allors
        [Id("EDFC927F-DFD4-4CAF-B7AE-7349A94BBF20")]
        [AssociationId("1844DCBB-6179-47C4-A5E4-C9D440B17551")]
        [RoleId("2E95C569-1D47-4195-A7FF-01848DA65559")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal UnitDiscount { get; set; }

        #region Allors
        [Id("35034B55-9903-4561-B195-19A7865D09BA")]
        [AssociationId("A4B3F363-3621-46F1-9CA2-E448DB26CCD6")]
        [RoleId("6A142464-A910-4A0F-82BD-C98B8F342378")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        VatRate VatRate { get; set; }

        #region Allors
        [Id("AE5ECB96-912C-49FC-9F99-4EC3D5B86406")]
        [AssociationId("EDCBD9B2-1D1A-4BD0-861F-2FFB275D5BF2")]
        [RoleId("4026E08B-9CCD-4A6E-A8E5-C6681087BFDB")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        decimal AssignedUnitPrice { get; set; }

        #region Allors
        [Id("BBA7B69A-7D79-4E6E-B39C-891A8DF36148")]
        [AssociationId("7BE8D57D-F4DC-4EA8-B387-2EEB5837A129")]
        [RoleId("326936A8-938E-49DE-A66D-6E3D68EF24CA")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal UnitBasePrice { get; set; }

        #region Allors
        [Id("5AABEDB7-9690-402A-9188-1929BAA75D89")]
        [AssociationId("A73DAD0C-70B4-4563-9BA3-6C3C2FA0EC4D")]
        [RoleId("A1224CD7-057F-4A0B-AEC7-9D48257D072D")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal UnitPrice { get; set; }

        #region Allors
        [Id("C75F67AF-3875-46F7-9B4D-C569799821E2")]
        [AssociationId("25A56FDC-89E9-4906-A3C0-ECD8E174C2C3")]
        [RoleId("2CD3E284-5D7E-4E18-A607-BB77A3595651")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("D2212BA5-C309-4909-B584-6B118D51DAB7")]
        [AssociationId("57FDA243-D4CC-4581-9E8F-DA070C28BAE5")]
        [RoleId("50B785DB-219C-4332-BECE-5273B0D518D3")]
        #endregion
        [Derived]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurchargeAsPercentage { get; set; }

        #region Allors
        [Id("9BA0BE40-56FB-4F50-AA08-B5FDE6D8B36B")]
        [AssociationId("ECD76828-D473-434A-A419-1D87D5A30003")]
        [RoleId("195C56D3-BF4E-4D67-834E-A402C1E458B7")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("785DD408-CE58-44FC-8F16-66FFBEA6DF0E")]
        [AssociationId("F8F0F4FF-0D67-46C4-83CA-05D2D3D670FD")]
        [RoleId("DDAA5787-67D6-45C2-8408-74861806C85B")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("6D73797C-EC9F-4C59-9234-4E67756AE365")]
        [AssociationId("E61F63DD-7D7A-406D-B78D-DE3E9283A63A")]
        [RoleId("A2B16FE4-CB97-4BA4-AB3C-6F456662D327")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        VatRegime AssignedVatRegime { get; set; }

        #region Allors
        [Id("5AE60592-1B86-4C71-860D-A3C0CFC1C647")]
        [AssociationId("BEB6CD77-C354-4581-BF79-9771C02E704A")]
        [RoleId("395C5DEE-F067-48ED-8A9F-2A93B397558A")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("D9E651C1-F508-402E-B599-6EE3206B5924")]
        [AssociationId("448C1C15-AC12-42D1-ABB0-E57B0221F354")]
        [RoleId("71D83C19-D873-4065-9DAC-9D1623FE4138")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("1817B33A-C8CD-4F87-BB4F-BFE0ABCCCF24")]
        [AssociationId("CB923F27-92A3-4C9F-B4D1-D1D3E801B5B7")]
        [RoleId("7CCA3632-FC96-4D03-ABB3-BD5B10FA8FF2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        SurchargeAdjustment SurchargeAdjustment { get; set; }
    }
}
