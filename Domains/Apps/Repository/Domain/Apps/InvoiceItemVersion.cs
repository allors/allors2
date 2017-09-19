namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("D8B0BB9D-BD15-426C-A4A4-6AEEFF2FBDB2")]
    #endregion
    public partial interface InvoiceItemVersion : PriceableVersion
    {
        #region Allors
        [Id("9D47BAEB-ED16-4314-84EA-4A462E554823")]
        [AssociationId("A239441B-0683-4FA5-A298-E6D90F7E389C")]
        [RoleId("B5F16EEA-BDD1-4182-84C4-E4F0E7FBFC17")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Size(-1)]
        [Workspace]
        string InternalComment { get; set; }

        #region Allors
        [Id("83241B88-9F30-4732-9755-03B27A6DC1F8")]
        [AssociationId("73EF285B-3BA2-46E8-9360-C4F93CD8E6F7")]
        [RoleId("7B04E9F9-8CF0-4BE3-9C8C-879B9B267FAC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        AgreementTerm[] InvoiceTerms { get; set; }

        #region Allors
        [Id("5C0780BC-5580-41AF-BA01-8643B93FF4F7")]
        [AssociationId("178F9CAE-7A1A-4FF9-9C60-0E569F8E4175")]
        [RoleId("CDB5C61B-8BA8-43C4-AFA5-D259CDC94FCB")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalInvoiceAdjustment { get; set; }

        #region Allors
        [Id("DA6D40BB-C45E-46A5-802A-94DFFB535870")]
        [AssociationId("704B58E6-D9CF-4FE6-BED7-2E5C5CBDF660")]
        [RoleId("FEBCD416-CB4C-4E7D-A3A4-0FCEA2D6703C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }

        #region Allors
        [Id("14C11E41-00AA-4406-9D55-887B2DF66C6C")]
        [AssociationId("D589C69B-BE2D-43FD-8E2F-DEDCC2DF500B")]
        [RoleId("62D80937-6DB6-402B-93C6-9C88E0662D57")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        InvoiceItem AdjustmentFor { get; set; }

        #region Allors
        [Id("E8E885EC-5F87-469A-9C5D-1A7F369D4D23")]
        [AssociationId("DC6E6C8E-6D6E-43FB-A228-34F221513A52")]
        [RoleId("6B5EB401-EF03-4824-BA5F-CDB3C162DE6E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        SerialisedInventoryItem SerializedInventoryItem { get; set; }

        #region Allors
        [Id("8738D82C-99E0-4EAE-BC4F-77B2F8D04E8D")]
        [AssociationId("9FBDF30A-F721-4AB7-8840-9A9EAF6D93A6")]
        [RoleId("E562AE69-D37F-4CBE-8DD1-FAB2C2E5D970")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Message { get; set; }

        #region Allors
        [Id("A34C9F0C-FD65-4B77-AB61-20442A110E9B")]
        [AssociationId("BB4F4CD5-CA0F-4C6F-8259-9A30F93305B7")]
        [RoleId("2872F5C8-0FDE-4780-BF22-0DBFC6A6971A")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalInvoiceAdjustmentCustomerCurrency { get; set; }

        #region Allors
        [Id("AD28ED60-187C-4722-A41F-2372B274B193")]
        [AssociationId("3C870438-6A7A-44FD-A072-A93E33D10DA2")]
        [RoleId("5F3CAA66-7D01-45F4-B377-65EBFFC10BF8")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountPaid { get; set; }

        #region Allors
        [Id("A6FBC8B3-2A1B-47B5-813C-B58C9C84FBDD")]
        [AssociationId("B0FC29A3-A2C7-49A6-BBE2-515C28780E96")]
        [RoleId("F1FE495D-D977-4DAF-AA3B-07B4AEA2DE38")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal Quantity { get; set; }

        #region Allors
        [Id("676378E4-F8AB-41AE-82F0-9F143F8A9A36")]
        [AssociationId("BB8D0924-177B-4DCA-883A-9873B51D92D6")]
        [RoleId("3B73D197-A40A-4322-9DC9-849C00A8EB47")]
        #endregion
        [Size(256)]
        [Workspace]
        string Description { get; set; }
    }
}