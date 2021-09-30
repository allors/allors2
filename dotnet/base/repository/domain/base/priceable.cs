// <copyright file="Priceable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("26e69a5f-0220-4b60-99bf-26e150bcb64c")]
    #endregion
    public partial interface Priceable : Commentable, Transitional, Object
    {
        #region Allors
        [Id("5ffe1fc4-704e-4a3f-a27f-d4a47c99c37b")]
        [AssociationId("7996c255-663f-462e-bb23-ae61a55a3a48")]
        [RoleId("827f06dd-fc4a-4323-9120-49f9a1ae9abf")]
        #endregion
        [Precision(19)]
        [Scale(4)]
        [Workspace]
        decimal AssignedUnitPrice { get; set; }

        #region Allors
        [Id("6dc95126-3287-46e0-9c21-4d6561262a2e")]
        [AssociationId("f041eec1-749c-4b73-a01a-c3d692a9d9db")]
        [RoleId("cf6e5ebb-4f3b-42e7-b6a3-486de6ca6d53")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(4)]
        [Workspace]
        decimal UnitBasePrice { get; set; }

        #region Allors
        [Id("7595b52c-012b-42db-9cf2-46939b39f57f")]
        [AssociationId("93e899b5-b472-4aea-9f7c-d0863883abb1")]
        [RoleId("c5f0b047-8a9b-4743-bac6-0d358b54a794")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(4)]
        [Workspace]
        decimal UnitPrice { get; set; }

        #region Allors
        [Id("3722807a-0634-4df2-8964-4778b4edc314")]
        [AssociationId("091b8400-d566-472d-a804-a55bfd99ff92")]
        [RoleId("7dcca5fa-73b5-4751-9d16-b05e6e5ef5b7")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal UnitSurcharge { get; set; }

        #region Allors
        [Id("37623a64-9f6c-4f35-8e72-c4332037db4d")]
        [AssociationId("c99eecb1-6b8a-4f44-999d-35b32ea93605")]
        [RoleId("cc34ad5e-43bd-4e4a-bd9a-afdb64a409ae")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal UnitDiscount { get; set; }

        #region Allors
        [Id("131359fb-29f2-4ebb-adc2-1e53a99a4e6b")]
        [AssociationId("e687dc65-d903-47c4-8e39-ad43f8d5633e")]
        [RoleId("a1031b6a-897b-43e4-99c9-0308acbe708b")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal UnitVat { get; set; }

        #region Allors
        [Id("c897fe12-da96-47e6-b00e-920cb9e1c790")]
        [AssociationId("40f8d741-df32-487e-8ca5-2764dcaa2200")]
        [RoleId("081c9f92-53c0-448a-bc8c-b19335f43da4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        VatRegime AssignedVatRegime { get; set; }

        #region Allors
        [Id("27f86828-7b4e-4d80-9c3c-095813240c1a")]
        [AssociationId("628cb976-30ef-42fd-be72-282b0f291bb2")]
        [RoleId("b78ae277-ca2b-43ff-a730-257281533822")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        VatRegime DerivedVatRegime { get; set; }

        #region Allors
        [Id("5367e41e-b1c3-4311-87b4-6ba2732de1e6")]
        [AssociationId("1f602ef8-dfa3-45f6-8577-e6256206bf94")]
        [RoleId("bf829774-be83-4c46-9174-dfeee0eb1fd7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        VatRate VatRate { get; set; }

        #region Allors
        [Id("b88638a1-4c91-4b50-80d8-430cf840c38b")]
        [AssociationId("95ba4300-41f0-4055-855b-62954db38101")]
        [RoleId("37fb4db0-9cfa-494c-8852-12e4ee1a96bb")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        DiscountAdjustment[] DiscountAdjustments { get; set; }

        #region Allors
        [Id("d4ea50dd-1e6e-44d2-8405-3a98a4b99104")]
        [AssociationId("8ca10731-f67d-424b-820c-fd01a1bae574")]
        [RoleId("9613b39d-bd0e-4520-a65e-e3997b389580")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        SurchargeAdjustment[] SurchargeAdjustments { get; set; }

        #region Allors
        [Id("d0b1e607-07dc-43e2-a003-89559c87a441")]
        [AssociationId("610d7c57-41eb-436f-b3ac-652798619441")]
        [RoleId("f17ef23b-b1c8-43e0-8adb-e605f7fef7ba")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(4)]
        [Workspace]
        decimal TotalBasePrice { get; set; }

        #region Allors
        [Id("dc71aecf-1735-4858-b74f-65e805565eed")]
        [AssociationId("e16d7c3d-628a-438f-b141-102d3d508380")]
        [RoleId("746ae197-8f1c-4631-8dcc-a7c9328f41e8")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal TotalExVat { get; set; }

        #region Allors
        [Id("32792771-06c8-4805-abc4-2e2f9c37c6f3")]
        [AssociationId("8f165d5a-ad87-431f-bea6-b8531f78d731")]
        [RoleId("dda45a1d-2377-40ad-88e7-0a8961c1f1e1")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal TotalVat { get; set; }

        #region Allors
        [Id("a271f7d4-cda1-4ae9-94e4-dda482bd8cd5")]
        [AssociationId("68f757fc-3cf5-4dae-b6ba-e2cb08033381")]
        [RoleId("b856b6e9-93e8-47ac-8070-3bb8c0ff29d7")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal TotalIncVat { get; set; }

        #region Allors
        [Id("0284a618-f661-4054-a09d-f94f9f778106")]
        [AssociationId("a11c49a8-6ace-47bf-afc2-a45b797e342b")]
        [RoleId("30a82cc2-ccf4-4ed2-a9a5-ee8074bda1f9")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal GrandTotal { get; set; }

        #region Allors
        [Id("b4398edb-2a36-459d-95a1-5d209462ae02")]
        [AssociationId("82b15a97-315c-4671-a420-f1b4f50f7ce6")]
        [RoleId("b561ab6f-8843-4409-ac10-accb4b6d123e")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal TotalDiscount { get; set; }

        #region Allors
        [Id("05254848-d99a-430e-80cd-e042ded3de71")]
        [AssociationId("b10824dd-70de-4fe1-bdc6-d970ebe33e4a")]
        [RoleId("c3de2ade-8b8b-4423-b40b-fa665a2d6215")]
        #endregion
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalDiscountAsPercentage { get; set; }

        #region Allors
        [Id("b81633d1-5b22-42b9-a484-d401d06022fb")]
        [AssociationId("17f4d6e4-fe43-46d4-b28d-651e6e766713")]
        [RoleId("e897b861-0e8a-4b57-ae52-b875d2f67c39")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(5)]
        [Workspace]
        decimal TotalSurcharge { get; set; }

        #region Allors
        [Id("a573b8bf-42a6-4389-9f46-1def243220bf")]
        [AssociationId("d70fe012-fbfb-486c-8ac7-ac3ae9ea380f")]
        [RoleId("6bff9fb4-7b17-4c5a-a7cb-fa8bd1bf9f5c")]
        #endregion
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalSurchargeAsPercentage { get; set; }
    }
}
