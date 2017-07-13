namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("a5e29ca1-80de-4de4-9085-b69f21550b5a")]
    #endregion
    public partial class VatRate : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0d6bd6c4-7220-45b4-891c-719f4bd141ce")]
        [AssociationId("f04be7c9-5f36-4cc2-8ad0-cad7386114da")]
        [RoleId("2f66e429-ac12-4d6c-9f06-a5986b0667fc")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public VatCalculationMethod VatCalculationMethod { get; set; }
        #region Allors
        [Id("36b9d86d-4e2e-4ff5-b167-8ea6c81dd6cc")]
        [AssociationId("8e37c73a-5508-432f-94c9-77d0159b0cc2")]
        [RoleId("955c2b54-1aab-4eb0-b71d-3b6e4664e3b3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public VatReturnBox[] VatReturnBoxes { get; set; }
        #region Allors
        [Id("3f1ca41a-8443-4d81-a112-48fa1e28728b")]
        [AssociationId("f95d3157-469c-454d-8e5b-57e52ac2c89c")]
        [RoleId("abf7d332-7a32-4b1f-91dd-3bd3802b8efa")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Rate { get; set; }
        #region Allors
        [Id("46cf5d68-cceb-4b49-933c-875e9614eb8b")]
        [AssociationId("deaf1d2c-6590-460e-9e16-0eaf68af6b3d")]
        [RoleId("b4fdf839-5b01-432c-b165-9664a199d0bf")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]

        public OrganisationGlAccount VatPayableAccount { get; set; }
        #region Allors
        [Id("5418fdea-366c-4e0b-b2e0-d49cfb12cbe5")]
        [AssociationId("b63a4251-c297-46cb-a2a3-b0d619abe398")]
        [RoleId("52fc90c1-2de7-4076-8cbf-44174ebd25a2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Organisation TaxAuthority { get; set; }
        #region Allors
        [Id("5551f4ce-858f-4f29-9e92-3c2c893bb44b")]
        [AssociationId("bf0f6d49-1753-42f4-99e6-649e64bb0629")]
        [RoleId("be2f8700-bd7d-4dd1-91ff-637ddc6a07a6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public VatRateUsage VatRateUsage { get; set; }
        #region Allors
        [Id("821df580-26d4-415c-b2ea-3e96a08c2f62")]
        [AssociationId("5678eef7-6892-4c47-900d-85b5c5c08940")]
        [RoleId("bb7c23b8-0a01-4afa-91a6-7816bbaa803c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public VatRatePurchaseKind VatRatePurchaseKind { get; set; }
        #region Allors
        [Id("8b37058f-49bd-4cc6-8c26-9a9e7c6700ad")]
        [AssociationId("71231b78-14e7-41c3-8691-745f4dd9c919")]
        [RoleId("00945490-ff10-440b-be3b-de563caf892f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public VatTariff VatTariff { get; set; }
        #region Allors
        [Id("958c1fda-0126-4b0a-8967-5d9df3ba50dc")]
        [AssociationId("edcb9612-a4d4-4ddc-971c-48b7dfa6b03c")]
        [RoleId("9e77b9ad-9031-4766-a2f3-33c875395a79")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public TimeFrequency PaymentFrequency { get; set; }
        #region Allors
        [Id("b2aa3989-8e65-4fdb-9654-46ae615fd73a")]
        [AssociationId("74a0152e-f989-4bf6-8164-7e515876a65a")]
        [RoleId("5b899c69-af8c-4655-9809-b4158738e1db")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public OrganisationGlAccount VatToPayAccount { get; set; }
        #region Allors
        [Id("b628964e-5139-4c32-a2c1-239deaff70e8")]
        [AssociationId("229bebd5-4899-4f7d-bebd-266ee211f72a")]
        [RoleId("c51def22-77d1-4dc5-bc3e-b55095ae5af1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public EuSalesListType EuSalesListType { get; set; }
        #region Allors
        [Id("cbd85372-08d1-4c6d-81a9-02d76c874c46")]
        [AssociationId("235cfb9b-1cea-4e37-8e35-f9993ca175b6")]
        [RoleId("41b72bc0-f2b6-4895-808b-76a5b6fb9035")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public OrganisationGlAccount VatToReceiveAccount { get; set; }
        #region Allors
        [Id("cf879781-9f52-438c-b0e0-fd23f336bead")]
        [AssociationId("524372df-0707-4a60-b5f6-1305d197da36")]
        [RoleId("c8831b10-b22f-49d4-a551-05b0c9f6ade2")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]

        public OrganisationGlAccount VatReceivableAccount { get; set; }
        #region Allors
        [Id("e6242c51-98f9-408d-9dd8-07e3c639c82e")]
        [AssociationId("11486112-1786-4d73-aee7-cdc1e8b271e3")]
        [RoleId("5224e2d8-7d48-4f63-97ff-796232781f81")]
        #endregion

        public bool ReverseCharge { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}