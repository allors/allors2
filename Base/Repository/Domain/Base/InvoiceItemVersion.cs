// <copyright file="InvoiceItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

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
        SalesTerm[] SalesTerms { get; set; }

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
        [Id("8738D82C-99E0-4EAE-BC4F-77B2F8D04E8D")]
        [AssociationId("9FBDF30A-F721-4AB7-8840-9A9EAF6D93A6")]
        [RoleId("E562AE69-D37F-4CBE-8DD1-FAB2C2E5D970")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Message { get; set; }

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
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("697df771-9dcd-4cfb-a4cb-fbe023aa9515")]
        [AssociationId("01a529c7-ca82-4b78-876c-f5d706d36902")]
        [RoleId("6bbb8e07-94b5-4540-8f10-c52b564844bd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        IrpfRegime IrpfRegime { get; set; }

        #region Allors
        [Id("caece89f-9811-4790-911d-54e5fda82265")]
        [AssociationId("d350c642-e1f3-4c6e-99e1-cff940c7c4ea")]
        [RoleId("a6a7fe0b-4a12-43cc-8e1b-999cc401dd07")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("f48e4902-aabd-4e2c-ad5f-4a45ea05426b")]
        [AssociationId("9bfc8075-f5d2-45b2-ba3c-b693cf894796")]
        [RoleId("582dc8dd-fa79-426b-8787-514f2a2e0d06")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        IrpfRate IrpfRate { get; set; }

        #region Allors
        [Id("1942c162-2af7-48f9-95ec-362697c5bb62")]
        [AssociationId("b0428e8c-601b-4ab3-b254-92fad17bb106")]
        [RoleId("0b1ed374-79dd-410b-ab12-ffa8add52825")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal UnitIrpf { get; set; }

        #region Allors
        [Id("d9463742-8414-441f-88ef-90938293296e")]
        [AssociationId("a219c33e-77dd-42a4-9ed3-bbc1f93bf8d8")]
        [RoleId("8d97d9fc-dac5-40b4-9c1f-ab74254d3519")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpf { get; set; }
    }
}
