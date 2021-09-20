// <copyright file="PartyFinancialRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("8F6C4557-AED7-4063-B05F-16573424FC51")]
    #endregion
    public partial class PartyFinancialRelationship : PartyRelationship, UniquelyIdentifiable
    {
        #region inherited properties

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Party[] Parties { get; set; }

        public Agreement[] Agreements { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("1A71A148-7F3D-4695-9546-63D282DA77D0")]
        [AssociationId("EDFF91B9-EFEC-4328-A19A-A6317C36BEAC")]
        [RoleId("6DB6134C-E5F6-4CBE-A73B-C38EDB3E7DC9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public Party Party { get; set; }

        #region Allors
        [Id("AB000DAF-93D2-43EE-8820-924575FEB098")]
        [AssociationId("09931678-1CEF-4DD6-B713-714D2827BEC1")]
        [RoleId("9F6F65D8-CCE0-46C6-A2A2-58D5B72C7A50")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation InternalOrganisation { get; set; }

        #region Allors
        [Id("01771db8-e79c-4ce4-9d81-db3675e8708a")]
        [AssociationId("c6dbe58e-fa09-408b-9324-21fcec3b1900")]
        [RoleId("aebbe259-2619-45bb-9751-68f61a230159")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal YTDRevenue { get; set; }

        #region Allors
        [Id("04bc4912-cd23-4b2e-973c-76bbf2f2de8d")]
        [AssociationId("c369193b-d01b-4f82-83f3-27ecaa3d8d58")]
        [RoleId("ef73d811-7d6a-4168-819f-1588b01979e8")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal LastYearsRevenue { get; set; }

        #region Allors
        [Id("52863081-34b7-48e2-a7ff-c6bd67172350")]
        [AssociationId("7ab0f4b0-f4ae-45d4-8c9e-a576f36e4f1a")]
        [RoleId("09d4533e-d118-4395-a7f1-358aad00f6e4")]
        #endregion
        [Workspace]
        public bool ExcludeFromDunning { get; set; }

        #region Allors
        [Id("d05ee314-57be-4852-a3b5-62710df4d4b7")]
        [AssociationId("87821f12-6fed-4376-b239-6d2296457b88")]
        [RoleId("a3a1df78-5469-41ae-bdc5-24c340abc378")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal OpenOrderAmount { get; set; }

        #region Allors
        [Id("d97ab83b-85dc-4877-8b49-1e552489bcb0")]
        [AssociationId("4af97ea0-bb6b-4fdb-9e0d-798805ccad53")]
        [RoleId("9c644a11-4239-49df-b603-489c547e2085")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod DefaultPaymentMethod { get; set; }

        #region Allors
        [Id("B66127AF-F490-4BF0-B8F5-6EBA878B1314")]
        [AssociationId("BBCAA840-593F-4155-8643-2E2FBA3E0035")]
        [RoleId("DF84CED1-8A60-43BF-A1A0-4C971F81C0B6")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal SimpleMovingAverage { get; set; }

        #region Allors
        [Id("42e3b2c4-376d-4e8b-bb49-2af031881ed0")]
        [AssociationId("bcdd31e8-8101-4b6b-8f13-a4397c43adfa")]
        [RoleId("a9ddfe04-e5fd-4b22-9a9a-702dc0533731")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AmountOverDue { get; set; }

        #region Allors
        [Id("76b46019-c145-403d-9f99-cd8e1001c968")]
        [AssociationId("6702ba13-81eb-4d23-b341-8fb84cf7e60f")]
        [RoleId("079e6188-73d0-4161-8327-607554a42613")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public DunningType DunningType { get; set; }

        #region Allors
        [Id("894f4ff2-9c41-4201-ad36-ac10dafd65dd")]
        [AssociationId("c8a336f0-4fae-4ce6-a900-283066052ffd")]
        [RoleId("11fa6c6e-c528-452c-adca-75f474d2f95b")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AmountDue { get; set; }

        #region Allors
        [Id("af50ade8-5964-4963-819d-c87689c6434e")]
        [AssociationId("a06dda1c-d91d-4e27-b293-05cb53de65ec")]
        [RoleId("7f6da6ca-b069-47f6-983c-6e33d65ffd0e")]
        #endregion
        [Workspace]
        public DateTime LastReminderDate { get; set; }

        #region Allors
        [Id("dd59ed76-b6da-49a3-8c3e-1edf4d1d0900")]
        [AssociationId("e2afe553-7bbd-4f81-97e8-7279defb49ca")]
        [RoleId("b5e30743-6adc-4bf0-b547-72b17b79879c")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal CreditLimit { get; set; }

        #region Allors
        [Id("e3a06a1c-998a-4871-8f0e-2f166eac6c7b")]
        [AssociationId("08dfdeb5-1a62-42d6-b8f3-16025960b09f")]
        [RoleId("9400c681-2a68-4842-89fd-3c9ccb3f2a96")]
        #endregion
        [Required]
        [Workspace]
        public int SubAccountNumber { get; set; }

        #region Allors
        [Id("ee871786-8840-404d-9b41-932a9f59be13")]
        [AssociationId("5b98959d-5589-4958-a86f-4c9b465c1632")]
        [RoleId("056ca61a-1ab4-4e53-8d5f-328ada5f3b11")]
        #endregion
        [Workspace]
        public DateTime BlockedForDunning { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
