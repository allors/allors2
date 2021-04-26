// <copyright file="OrderVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("2D3761C6-CF53-4CA8-9392-99ED6B01EDE1")]
    #endregion
    public partial interface OrderVersion : Version
    {
        #region Allors
        [Id("C1A57736-7027-4944-BF06-7CB5E513CCBE")]
        [AssociationId("89D7C360-0FBE-4ECF-B23F-5A507675E228")]
        [RoleId("E89E5DF1-923C-4F6C-9CFD-1920512DF445")]
        [Indexed]
        #endregion
        [Size(-1)]
        [Workspace]
        string Comment { get; set; }

        #region Allors
        [Id("CECB258A-5B25-4AB5-9F27-924CB9CD8080")]
        [AssociationId("C4A0106D-05F1-4663-AC6F-9D47E6F8C01C")]
        [RoleId("3976F31A-8981-44C9-82FD-2845ECFB3E3A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User CreatedBy { get; set; }

        #region Allors
        [Id("95FAE69F-0298-425A-AB74-7D553056624A")]
        [AssociationId("161AB23A-9B8A-4CDE-A6DC-F139F5E85D08")]
        [RoleId("0BD0FE07-F983-474C-9A60-B6D048513854")]
        #endregion
        [Workspace]
        DateTime CreationDate { get; set; }

        #region Allors
        [Id("16365C4A-C5F8-4004-9384-77E4E42E6151")]
        [AssociationId("7123418D-C368-4E8B-BE8A-609FDB08DE6C")]
        [RoleId("9393293D-05EC-4104-A7BC-B6DD9B54EEE3")]
        #endregion
        [Workspace]
        DateTime LastModifiedDate { get; set; }

        #region Allors
        [Id("812FD6D2-13B6-42A8-AE84-355E6DE25D86")]
        [AssociationId("06F8AFDB-626C-46E7-97EE-947243F0EBFB")]
        [RoleId("FC7B972C-C44A-47CD-87C7-B867EC79075F")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("60017934-BF46-485D-8844-E2FC521B6604")]
        [AssociationId("81C7D03B-B030-4DB2-8B3D-19BB4AE00B34")]
        [RoleId("5263F441-F790-4ECB-8118-E443139AD3C3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Currency AssignedCurrency { get; set; }

        #region Allors
        [Id("B3F0CB65-DEDF-4E81-998E-7766E8AC63C9")]
        [AssociationId("38AC3BD7-FF30-4693-8561-E150E8CDA4DC")]
        [RoleId("F3EEE845-7AB3-4F12-BDBD-33A4AF486DA1")]
        #endregion
        [Workspace]
        [Size(256)]
        string CustomerReference { get; set; }

        #region Allors
        [Id("4A6AC333-A672-459C-929D-A999A8DBBF08")]
        [AssociationId("D6D08D9B-7FBE-432E-8ED4-6A7DF741277B")]
        [RoleId("2BAA62A0-B829-4072-BD24-3368915F9B95")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("2d710be9-39be-4a8b-a88d-ceb960691acb")]
        [AssociationId("62cab763-bd70-4459-b839-0521f1ab0184")]
        [RoleId("1be9c62d-6485-436c-8355-0c8ad9e279f6")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        OrderAdjustment[] OrderAdjustments { get; set; }

        #region Allors
        [Id("83929DB1-5DFC-4D9F-A10E-C46CBD55ACA1")]
        [AssociationId("8862D446-F3B2-4225-BEC8-CA44D57C3296")]
        [RoleId("8248B489-7CE6-4848-8BC9-41316F5A3981")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToMany)]
        SalesTerm[] SalesTerms { get; set; }

        #region Allors
        [Id("057978E2-F3A3-4BD6-BF1C-A6D27EC6C154")]
        [AssociationId("9FC2013A-6A9A-420D-BF6B-6B5A91F98468")]
        [RoleId("C19348D1-7298-4F87-9520-D0A84A6BF451")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("24c94389-7016-433c-81b0-d63eea705d20")]
        [AssociationId("46218a54-5a81-4eb6-b4b3-1ae67765ac75")]
        [RoleId("cf3b1523-4eeb-4d44-8f00-d99040600a40")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpf { get; set; }

        #region Allors
        [Id("12DE1B8C-8488-4C18-B048-753640329AA6")]
        [AssociationId("3D217197-F1B2-4E8B-8E2F-56AD7FB92D9B")]
        [RoleId("9501778E-C9A2-4063-A5F4-746B3E227780")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("AD239348-CADB-4784-888A-68DBD082B7AA")]
        [AssociationId("ECC0F0A8-3E2A-44CC-931C-FFB235C33F4A")]
        [RoleId("F06A0AAB-F367-44C7-8AD0-9F8FFB4DC581")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        OrderItem[] ValidOrderItems { get; set; }

        #region Allors
        [Id("EC48639A-EB57-465D-8FE7-68BF15586F6B")]
        [AssociationId("1F48F823-580C-49C2-B0A1-4785730110E5")]
        [RoleId("70F8ED65-1FBD-4280-A508-1873176EC048")]
        #endregion
        [Workspace]
        [Size(256)]
        string OrderNumber { get; set; }

        #region Allors
        [Id("23891E65-CF51-4B93-893A-BA2D17346F67")]
        [AssociationId("3BAF6B85-1B7C-4047-8694-48C02AF113B2")]
        [RoleId("772B7762-F9FA-4A9C-AC1F-0AB84DCA65C5")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("654AB214-8214-4F86-9AE0-3DDCF8C10031")]
        [AssociationId("99E2D874-105A-41A5-9E26-396C33BB9C01")]
        [RoleId("CFBB683E-5419-4EF1-A29D-E01190B7B04A")]
        #endregion
        [Workspace]
        [Size(-1)]
        string Message { get; set; }

        #region Allors
        [Id("6D2AE22D-39BC-4A92-BE31-F3C30326498B")]
        [AssociationId("33973973-8E9C-49EA-ADA0-B6B5D78C6C86")]
        [RoleId("C06385B2-A212-4386-9540-143B31254E1B")]
        #endregion
        [Workspace]
        DateTime EntryDate { get; set; }

        #region Allors
        [Id("B6C314CD-6BBE-4D6A-9FDC-F19A18C128D8")]
        [AssociationId("5F77DA29-F781-4220-832C-F09D08F364C0")]
        [RoleId("3B383E2A-6F59-47AC-8E60-562B31E9F79F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        OrderKind OrderKind { get; set; }

        #region Allors
        [Id("5963F652-CA7A-4871-AD3C-DD31B8AD3385")]
        [AssociationId("85AB0037-3BA4-4428-A5EA-5B20BD5E0C68")]
        [RoleId("9AB06BCE-D956-4315-B94A-F0CC21748990")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("7166CBA0-82B6-4332-9D52-3874CDAF72CD")]
        [AssociationId("8442B5BA-8B63-46A1-8B0A-3D2B76797F9A")]
        [RoleId("E8F069AB-0261-46CA-9389-C70822F52BA0")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        VatRegime AssignedVatRegime { get; set; }

        #region Allors
        [Id("3606221e-2a60-406f-9829-4c9e6efad01d")]
        [AssociationId("e6673757-5409-4d6c-8f1b-95a86e46b4e9")]
        [RoleId("69108a4d-f1b2-4d50-88fa-9b9c890cdeee")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        VatRegime DerivedVatRegime { get; set; }

        #region Allors
        [Id("360268c9-501e-4f13-8ea0-01cce00197f5")]
        [AssociationId("802ceb59-d77d-4150-bd3a-e0546c83a46d")]
        [RoleId("5f8aeadb-a12e-47df-88e2-f701b0afb5a0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime AssignedIrpfRegime { get; set; }

        #region Allors
        [Id("edde2f2b-d3b8-4ff9-ab70-5103fd052977")]
        [AssociationId("8b67a52f-1eee-47a8-b473-a933f266b4f8")]
        [RoleId("0a3e4e75-1f18-42f5-805d-1bdb5d6c5920")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        IrpfRegime DerivedIrpfRegime { get; set; }

        #region Allors
        [Id("1ABA537A-2AC5-48BE-A4CA-D39B8811F10E")]
        [AssociationId("0159AD78-87CE-4034-B800-C4F84CB2CC22")]
        [RoleId("23138D1E-86D7-4CBE-B755-05ACE485EB64")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("eb5fad3d-e047-4efc-bd2e-b9b874dc6cc8")]
        [AssociationId("2745add8-f0a5-447d-9535-68e7ebf5d959")]
        [RoleId("3ffd6dad-9541-4804-a7c6-83d53373d252")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotal { get; set; }

        #region Allors
        [Id("D2B76765-60EC-4D08-9AE5-26398DC0A7A2")]
        [AssociationId("94AD9B68-9045-4678-879F-5636984364E0")]
        [RoleId("CBCADF37-5C40-47C2-9568-820BB0653CB8")]
        #endregion
        [Workspace]
        DateTime OrderDate { get; set; }

        #region Allors
        [Id("393069B8-9180-41B7-9D5D-40DC0EF16928")]
        [AssociationId("CCFE1910-D559-43FE-BA3E-9C930BC30DFF")]
        [RoleId("DBA91B3E-ECF2-4A5E-AD72-C038B5729E02")]
        #endregion
        [Workspace]
        DateTime DeliveryDate { get; set; }

        #region Allors
        [Id("49289FBF-480E-49BC-AE23-555F95D4D868")]
        [AssociationId("8B2E4717-6E21-4FE0-864F-C965AF2B1C0B")]
        [RoleId("C9F2F65D-A1D6-48C5-BD26-25D0FC6AE00B")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("20833465-873E-4DA6-B769-E8CD56A4D809")]
        [AssociationId("A1863AD4-8E77-4E97-9EF4-9F185F7094D8")]
        [RoleId("6C5DB3D1-4C76-4F9D-A9DF-6CDD3CFD72A9")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFee { get; set; }

        #region Allors
        [Id("c53a554d-eb77-4247-8eb7-ae5b353da13f")]
        [AssociationId("26e6d7d6-be03-4940-9da7-1857e18fddfb")]
        [RoleId("a89917a7-93a1-4fc9-abdd-904036233315")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraCharge { get; set; }

        #region Allors
        [Id("2be7e4c6-7c6f-40c9-908d-7e8998f498e7")]
        [AssociationId("95330aa2-4835-43aa-a66f-9b1c77cc5c47")]
        [RoleId("5e8f647c-a2aa-4844-92b7-93c85178e629")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalIrpfInPreferredCurrency { get; set; }

        #region Allors
        [Id("947906e5-e45d-4827-b866-787274b6c91c")]
        [AssociationId("eb154d8e-4516-4113-9e7d-9947c3b3c5f8")]
        [RoleId("2f463505-7fcf-459f-b828-61016f5b2d5e")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("67a766ac-d3e7-43b4-9af3-9a0f6b1378de")]
        [AssociationId("8401e440-dca9-4513-938f-aad2b264f238")]
        [RoleId("0126a4ca-cfb4-4660-8990-071df35afd5e")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("9b19b27a-b05b-4624-86b9-638fa2516d6d")]
        [AssociationId("93e27ba6-b475-4604-ad2f-35c498d08607")]
        [RoleId("5c8c9a27-3d4a-4042-8234-a055117b0b12")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalIncVatInPreferredCurrency { get; set; }

        #region Allors
        [Id("deb8b328-30c9-44dc-be73-dbe1c9821a43")]
        [AssociationId("e521e400-6f06-47cf-968c-94b026182b72")]
        [RoleId("d325186a-907c-410f-bcbc-d02d7dfac0f8")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalSurchargeInPreferredCurrency { get; set; }

        #region Allors
        [Id("8162085b-dd62-4346-9ab3-4219a761003f")]
        [AssociationId("56b73083-34fe-4ba8-af50-d32af4df795a")]
        [RoleId("9bbf9aa1-34b5-4475-aeef-a4937494d5b1")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalDiscountInPreferredCurrency { get; set; }

        #region Allors
        [Id("ec64d948-8571-4c03-90fa-3ae6ae8c62bf")]
        [AssociationId("9a0885be-8b68-437e-b2f5-9df50c818678")]
        [RoleId("7aa96c45-ae80-423c-9fc4-af05e5b2d7d0")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandlingInPreferredCurrency { get; set; }

        #region Allors
        [Id("157c7464-5acc-444b-9c16-33076d8b4ea4")]
        [AssociationId("34a79970-6211-4527-888c-f4f76d0f6054")]
        [RoleId("eff0c83d-aed1-4bee-9824-cb371e76a417")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFeeInPreferredCurrency { get; set; }

        #region Allors
        [Id("a5a198bf-b07b-4d71-a166-cd14c7cc120e")]
        [AssociationId("9506be54-a857-448b-92a6-4455fb353404")]
        [RoleId("bada20f6-31dd-4672-a076-fd952e214c9d")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalExtraChargeInPreferredCurrency { get; set; }

        #region Allors
        [Id("033b8379-6f93-4915-b4fb-a9d27b205f3f")]
        [AssociationId("f3405682-f523-4301-8f94-4dcc390c7e60")]
        [RoleId("af7d9138-4e90-4c0f-b316-bb410968e912")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePriceInPreferredCurrency { get; set; }

        #region Allors
        [Id("a6bf479b-d092-4a99-af26-91fe8c1b03d3")]
        [AssociationId("a575cf88-4b56-4ff3-a08c-b3a851ad1602")]
        [RoleId("95508981-9bd6-4d0c-b309-4cf3a931147f")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPriceInPreferredCurrency { get; set; }

        #region Allors
        [Id("01340397-9f93-4137-8f2a-93fe063c96db")]
        [AssociationId("bbf7c512-a1f7-44c1-be74-cf4a008be265")]
        [RoleId("75400f26-e8ba-4ea4-9d1b-9720095b2f92")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotalInPreferredCurrency { get; set; }
    }
}
