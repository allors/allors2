namespace Allors.Meta
{
    using System;

    #region Allors
    [Id("26E69A5F-0220-4B60-99BF-26E150BCB64C")]
    #endregion
    [Plural("Priceables")]
    [Inherit(typeof(AccessControlledObjectInterface))]
    [Inherit(typeof(CommentableInterface))]
    [Inherit(typeof(TransitionalInterface))]

    public partial class PriceableInterface : Interface
    {
        #region Allors
        [Id("05254848-d99a-430e-80cd-e042ded3de71")]
        [AssociationId("b10824dd-70de-4fe1-bdc6-d970ebe33e4a")]
        [RoleId("c3de2ade-8b8b-4423-b40b-fa665a2d6215")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalDiscountsAsPercentage")]
        public RelationType TotalDiscountAsPercentage;

        #region Allors
        [Id("0dc8733d-816a-4231-8a56-24363923080f")]
        [AssociationId("f41fe55a-b9f4-4a81-a7c6-cffb5e3e8cc1")]
        [RoleId("8aa28a5f-d801-4751-b37a-435b461b1b54")]
        #endregion
        [Indexed]
        [Type(typeof(DiscountAdjustmentClass))]
        [Plural("DiscountAdjustments")]
        [Multiplicity(Multiplicity.OneToOne)]
        public RelationType DiscountAdjustment;

        #region Allors
        [Id("131359fb-29f2-4ebb-adc2-1e53a99a4e6b")]
        [AssociationId("e687dc65-d903-47c4-8e39-ad43f8d5633e")]
        [RoleId("a1031b6a-897b-43e4-99c9-0308acbe708b")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("UnitsVat")]
        public RelationType UnitVat;

        #region Allors
        [Id("27534e6f-55d3-45e3-82ba-1580af4647d6")]
        [AssociationId("cda2b9ab-8d74-471c-95ea-38fb0c4a7589")]
        [RoleId("32f1c0f7-59d6-4c2c-a39e-607d359b6f53")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalVatsCustomerCurrency")]
        public RelationType TotalVatCustomerCurrency;

        #region Allors
        [Id("27f86828-7b4e-4d80-9c3c-095813240c1a")]
        [AssociationId("628cb976-30ef-42fd-be72-282b0f291bb2")]
        [RoleId("b78ae277-ca2b-43ff-a730-257281533822")]
        #endregion
        [Derived]
        [Indexed]
        [Type(typeof(VatRegimeClass))]
        [Plural("VatRegimes")]
        [Multiplicity(Multiplicity.ManyToOne)]
        public RelationType VatRegime;

        #region Allors
        [Id("32792771-06c8-4805-abc4-2e2f9c37c6f3")]
        [AssociationId("8f165d5a-ad87-431f-bea6-b8531f78d731")]
        [RoleId("dda45a1d-2377-40ad-88e7-0a8961c1f1e1")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalVats")]
        public RelationType TotalVat;

        #region Allors
        [Id("3722807a-0634-4df2-8964-4778b4edc314")]
        [AssociationId("091b8400-d566-472d-a804-a55bfd99ff92")]
        [RoleId("7dcca5fa-73b5-4751-9d16-b05e6e5ef5b7")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("UnitSurcharges")]
        public RelationType UnitSurcharge;

        #region Allors
        [Id("37623a64-9f6c-4f35-8e72-c4332037db4d")]
        [AssociationId("c99eecb1-6b8a-4f44-999d-35b32ea93605")]
        [RoleId("cc34ad5e-43bd-4e4a-bd9a-afdb64a409ae")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("UnitDiscounts")]
        public RelationType UnitDiscount;

        #region Allors
        [Id("47839962-efc3-4def-be94-4a5831c3a629")]
        [AssociationId("061745f4-94f0-4370-ad60-d08e46d6d474")]
        [RoleId("eef09133-a624-455e-81f4-7c84ea41c931")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalExVatsCustomerCurrency")]
        public RelationType TotalExVatCustomerCurrency;

        #region Allors
        [Id("5367e41e-b1c3-4311-87b4-6ba2732de1e6")]
        [AssociationId("1f602ef8-dfa3-45f6-8577-e6256206bf94")]
        [RoleId("bf829774-be83-4c46-9174-dfeee0eb1fd7")]
        #endregion
        [Derived]
        [Indexed]
        [Type(typeof(VatRateClass))]
        [Plural("DerivedVatRates")]
        [Multiplicity(Multiplicity.ManyToOne)]
        public RelationType DerivedVatRate;

        #region Allors
        [Id("5ffe1fc4-704e-4a3f-a27f-d4a47c99c37b")]
        [AssociationId("7996c255-663f-462e-bb23-ae61a55a3a48")]
        [RoleId("827f06dd-fc4a-4323-9120-49f9a1ae9abf")]
        #endregion
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("ActualUnitPrices")]
        public RelationType ActualUnitPrice;

        #region Allors
        [Id("6d1a448e-112a-4513-87b8-fd4e5bd03dac")]
        [AssociationId("0fe2ff69-d63c-4361-acb3-4fd10ddf30bc")]
        [RoleId("f38daa9a-0303-4dba-9c92-8483f8a134c4")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalIncVatsCustomerCurrency")]
        public RelationType TotalIncVatCustomerCurrency;

        #region Allors
        [Id("6dc95126-3287-46e0-9c21-4d6561262a2e")]
        [AssociationId("f041eec1-749c-4b73-a01a-c3d692a9d9db")]
        [RoleId("cf6e5ebb-4f3b-42e7-b6a3-486de6ca6d53")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("UnitBasePrices")]
        public RelationType UnitBasePrice;

        #region Allors
        [Id("7595b52c-012b-42db-9cf2-46939b39f57f")]
        [AssociationId("93e899b5-b472-4aea-9f7c-d0863883abb1")]
        [RoleId("c5f0b047-8a9b-4743-bac6-0d358b54a794")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("CalculatedUnitPrice")]
        public RelationType CalculatedUnitPrice;

        #region Allors
        [Id("8f3d28ac-7693-4943-9398-a30f3f957283")]
        [AssociationId("035e178a-7de0-45c8-a4c1-269eee4c3f0c")]
        [RoleId("3ed702c2-bf3c-472b-8a00-829c0853b7f7")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalSurchargesCustomerCurrency")]
        public RelationType TotalSurchargeCustomerCurrency;

        #region Allors
        [Id("a271f7d4-cda1-4ae9-94e4-dda482bd8cd5")]
        [AssociationId("68f757fc-3cf5-4dae-b6ba-e2cb08033381")]
        [RoleId("b856b6e9-93e8-47ac-8070-3bb8c0ff29d7")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotaIIncVats")]
        public RelationType TotalIncVat;

        #region Allors
        [Id("a573b8bf-42a6-4389-9f46-1def243220bf")]
        [AssociationId("d70fe012-fbfb-486c-8ac7-ac3ae9ea380f")]
        [RoleId("6bff9fb4-7b17-4c5a-a7cb-fa8bd1bf9f5c")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalSurchargesAsPercentage")]
        public RelationType TotalSurchargeAsPercentage;

        #region Allors
        [Id("a819e4fe-f829-4e1c-9e93-46d9c4b31bd4")]
        [AssociationId("1f3b767e-58ee-483d-aa18-ee5c7421a244")]
        [RoleId("b0b1feaa-279f-488a-9062-e0b1ff2dcd7c")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalDiscountsCustomerCurrency")]
        public RelationType TotalDiscountCustomerCurrency;

        #region Allors
        [Id("b4398edb-2a36-459d-95a1-5d209462ae02")]
        [AssociationId("82b15a97-315c-4671-a420-f1b4f50f7ce6")]
        [RoleId("b561ab6f-8843-4409-ac10-accb4b6d123e")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalDiscounts")]
        public RelationType TotalDiscount;

        #region Allors
        [Id("b81633d1-5b22-42b9-a484-d401d06022fb")]
        [AssociationId("17f4d6e4-fe43-46d4-b28d-651e6e766713")]
        [RoleId("e897b861-0e8a-4b57-ae52-b875d2f67c39")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalSurcharges")]
        public RelationType TotalSurcharge;

        #region Allors
        [Id("c897fe12-da96-47e6-b00e-920cb9e1c790")]
        [AssociationId("40f8d741-df32-487e-8ca5-2764dcaa2200")]
        [RoleId("081c9f92-53c0-448a-bc8c-b19335f43da4")]
        #endregion
        [Indexed]
        [Type(typeof(VatRegimeClass))]
        [Plural("AssignedVatRegimes")]
        [Multiplicity(Multiplicity.ManyToOne)]
        public RelationType AssignedVatRegime;

        #region Allors
        [Id("d0b1e607-07dc-43e2-a003-89559c87a441")]
        [AssociationId("610d7c57-41eb-436f-b3ac-652798619441")]
        [RoleId("f17ef23b-b1c8-43e0-8adb-e605f7fef7ba")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalBasePrices")]
        public RelationType TotalBasePrice;

        #region Allors
        [Id("dc71aecf-1735-4858-b74f-65e805565eed")]
        [AssociationId("e16d7c3d-628a-438f-b141-102d3d508380")]
        [RoleId("746ae197-8f1c-4631-8dcc-a7c9328f41e8")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalExVats")]
        public RelationType TotalExVat;

        #region Allors
        [Id("dc7d46b9-7c46-42bf-b8ac-20065a958c51")]
        [AssociationId("d935d887-08c0-499b-be79-37973dac97e5")]
        [RoleId("edfe7ff0-3a6d-40b4-ab75-86d4b231de95")]
        #endregion
        [Derived]
        [Type(typeof(AllorsDecimalUnit))]
        [Precision(19)]
        [Scale(2)]
        [Plural("TotalBasePricesCustomerCurrency")]
        public RelationType TotalBasePriceCustomerCurrency;

        #region Allors
        [Id("dcc8e49e-5770-4686-8f3c-ecedf5bbbfed")]
        [AssociationId("b855278d-96ab-486d-b12b-71e2ffe8353d")]
        [RoleId("1bdfc536-bdcc-41dc-b7d3-357c4bcc24cf")]
        #endregion
        [Derived]
        [Indexed]
        [Type(typeof(PriceComponentInterface))]
        [Plural("CurrentPriceComponents")]
        [Multiplicity(Multiplicity.ManyToMany)]
        public RelationType CurrentPriceComponent;

        #region Allors
        [Id("fa02ba8e-24a6-45ca-acfc-9ef69301efa2")]
        [AssociationId("fc87d284-a120-43fa-86eb-f4aea034cbf4")]
        [RoleId("97d6f184-64c7-4ec7-953e-7ff587cd29af")]
        #endregion
        [Indexed]
        [Type(typeof(SurchargeAdjustmentClass))]
        [Plural("SurchargeAdjustments")]
        [Multiplicity(Multiplicity.OneToOne)]
        public RelationType SurchargeAdjustment;

        public static PriceableInterface Instance { get; internal set; }

        internal PriceableInterface() : base(MetaPopulation.Instance)
        {
        }
    }
}