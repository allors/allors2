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
        [Derived]
        [Indexed]
        Currency Currency { get; set; }

        #region Allors
        [Id("B3F0CB65-DEDF-4E81-998E-7766E8AC63C9")]
        [AssociationId("38AC3BD7-FF30-4693-8561-E150E8CDA4DC")]
        [RoleId("F3EEE845-7AB3-4F12-BDBD-33A4AF486DA1")]
        #endregion
        [Workspace]
        [Size(256)]
        string CustomerReference { get; set; }

        #region Allors
        [Id("826CBCB8-EEA9-4FCF-9D96-47B28E350C6C")]
        [AssociationId("532FBA10-583B-4C91-B514-31CC800110DE")]
        [RoleId("89061789-4862-4D64-84D2-BB60FE34F698")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        Fee Fee { get; set; }

        #region Allors
        [Id("4A6AC333-A672-459C-929D-A999A8DBBF08")]
        [AssociationId("D6D08D9B-7FBE-432E-8ED4-6A7DF741277B")]
        [RoleId("2BAA62A0-B829-4072-BD24-3368915F9B95")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("83929DB1-5DFC-4D9F-A10E-C46CBD55ACA1")]
        [AssociationId("8862D446-F3B2-4225-BEC8-CA44D57C3296")]
        [RoleId("8248B489-7CE6-4848-8BC9-41316F5A3981")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        SalesTerm[] SalesTerms { get; set; }

        #region Allors
        [Id("057978E2-F3A3-4BD6-BF1C-A6D27EC6C154")]
        [AssociationId("9FC2013A-6A9A-420D-BF6B-6B5A91F98468")]
        [RoleId("C19348D1-7298-4F87-9520-D0A84A6BF451")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("12DE1B8C-8488-4C18-B048-753640329AA6")]
        [AssociationId("3D217197-F1B2-4E8B-8E2F-56AD7FB92D9B")]
        [RoleId("9501778E-C9A2-4063-A5F4-746B3E227780")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
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
        [Derived]
        [Indexed]
        OrderItem[] ValidOrderItems { get; set; }

        #region Allors
        [Id("EC48639A-EB57-465D-8FE7-68BF15586F6B")]
        [AssociationId("1F48F823-580C-49C2-B0A1-4785730110E5")]
        [RoleId("70F8ED65-1FBD-4280-A508-1873176EC048")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Size(256)]
        string OrderNumber { get; set; }

        #region Allors
        [Id("23891E65-CF51-4B93-893A-BA2D17346F67")]
        [AssociationId("3BAF6B85-1B7C-4047-8694-48C02AF113B2")]
        [RoleId("772B7762-F9FA-4A9C-AC1F-0AB84DCA65C5")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
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
        [Derived]
        [Required]
        DateTime EntryDate { get; set; }

        #region Allors
        [Id("8A64D902-CE14-4D05-9AA5-FA0747309ED3")]
        [AssociationId("7441AE7B-8AB5-4273-ADEF-1D77BB56CA19")]
        [RoleId("9A5260E2-49C2-4BAB-8A26-7E165938C5FB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        DiscountAdjustment DiscountAdjustment { get; set; }

        #region Allors
        [Id("B6C314CD-6BBE-4D6A-9FDC-F19A18C128D8")]
        [AssociationId("5F77DA29-F781-4220-832C-F09D08F364C0")]
        [RoleId("3B383E2A-6F59-47AC-8E60-562B31E9F79F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        OrderKind OrderKind { get; set; }

        #region Allors
        [Id("5963F652-CA7A-4871-AD3C-DD31B8AD3385")]
        [AssociationId("85AB0037-3BA4-4428-A5EA-5B20BD5E0C68")]
        [RoleId("9AB06BCE-D956-4315-B94A-F0CC21748990")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
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
        VatRegime VatRegime { get; set; }

        #region Allors
        [Id("1ABA537A-2AC5-48BE-A4CA-D39B8811F10E")]
        [AssociationId("0159AD78-87CE-4034-B800-C4F84CB2CC22")]
        [RoleId("23138D1E-86D7-4CBE-B755-05ACE485EB64")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalShippingAndHandling { get; set; }

        #region Allors
        [Id("45F0FEB5-6D0F-4FB8-AC5F-C85E5D4BA32E")]
        [AssociationId("98E772C0-0E93-4CC0-AB1D-2D956AF5F1A4")]
        [RoleId("1640BAEC-B8BA-4205-99C2-1F4383B0680D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }

        #region Allors
        [Id("D2B76765-60EC-4D08-9AE5-26398DC0A7A2")]
        [AssociationId("94AD9B68-9045-4678-879F-5636984364E0")]
        [RoleId("CBCADF37-5C40-47C2-9568-820BB0653CB8")]
        #endregion
        [Workspace]
        [Required]
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
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("20833465-873E-4DA6-B769-E8CD56A4D809")]
        [AssociationId("A1863AD4-8E77-4E97-9EF4-9F185F7094D8")]
        [RoleId("6C5DB3D1-4C76-4F9D-A9DF-6CDD3CFD72A9")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal TotalFee { get; set; }

        #region Allors
        [Id("CFC6FD15-37EE-455F-BE57-00B1C3FF2481")]
        [AssociationId("D63DAF25-07D7-4F68-B1A0-D671C533CB57")]
        [RoleId("A03CDFDC-F5C2-4225-805B-67B8D00C83EB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        SurchargeAdjustment SurchargeAdjustment { get; set; }
    }
}
