// <copyright file="FiscalYearOrganisationSequenceNumbers.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("0519ac3d-898d-4880-9019-7d47eb650a88")]
    #endregion
    [Plural("FiscalYearsInternalOrganisationSequenceNumbers")]

    public partial class FiscalYearInternalOrganisationSequenceNumbers : Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("05b4f4a5-2e87-47e8-a665-47449c56cc05")]
        [AssociationId("4a44662f-f28a-4169-b2ce-cb8da09de250")]
        [RoleId("3f0a988e-a53a-4a45-a095-3d4b2b10f2e2")]
        #endregion
        [Required]
        public int FiscalYear { get; set; }

        #region Allors
        [Id("26e27fd4-b5ce-45ac-9643-52e0537b6601")]
        [AssociationId("1d806c25-2a3d-4a50-a228-f01d788da5ff")]
        [RoleId("63e2fd37-9326-49d7-b2d2-4e34c2979ddc")]
        #endregion
        [Size(256)]
        [Workspace]
        public string PurchaseInvoiceNumberPrefix { get; set; }

        #region Allors
        [Id("fdabc29e-ee35-44c0-a9bf-8d789d06ab46")]
        [AssociationId("fd79cb48-f076-45d2-b1c6-7e8483cfe508")]
        [RoleId("45bf9f09-86ed-403b-972e-d6ed60985b87")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Counter PurchaseInvoiceNumberCounter { get; set; }

        #region Allors
        [Id("4a3909c6-3443-425b-89f9-18e3df46d7dd")]
        [AssociationId("18378d30-2096-4eba-91cc-a550bd767c52")]
        [RoleId("1d26e413-7154-4411-ba66-3270831c23e7")]
        #endregion
        [Size(256)]
        [Workspace]
        public string PurchaseOrderNumberPrefix { get; set; }

        #region Allors
        [Id("d241e5f1-7dd9-442f-90c1-67707b92bcb6")]
        [AssociationId("21637835-b9f3-49dd-bf9b-315a0346d134")]
        [RoleId("fc887d98-5806-4d57-8b74-743873fe2b0b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Counter PurchaseOrderNumberCounter { get; set; }

        #region Allors
        [Id("2afe1843-a604-427d-aaf0-486153fe7a96")]
        [AssociationId("5285b94e-065f-45fe-b22b-6fd8e1b1b1e1")]
        [RoleId("c9488136-975a-4e4a-b39c-54d5cbf53966")]
        #endregion
        [Size(256)]
        [Workspace]
        public string RequestNumberPrefix { get; set; }

        #region Allors
        [Id("7fd7a805-651a-48e0-93d8-ce595dcc4411")]
        [AssociationId("aaf33c4f-f722-41bb-aa3e-79570d0d89d9")]
        [RoleId("d45450b5-11a9-4bd6-89a9-c4ba027a6043")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Counter RequestNumberCounter { get; set; }

        #region Allors
        [Id("91c177da-eb89-4d6f-8d51-d8e98e855faa")]
        [AssociationId("af006d15-7a3a-4cd8-aeb5-6de92bfd3c8a")]
        [RoleId("3fe145c5-d893-4136-89f2-b3df37a4965e")]
        #endregion
        [Workspace]
        [Size(256)]
        public string IncomingShipmentNumberPrefix { get; set; }

        #region Allors
        [Id("96201883-7ccb-4fea-9958-de7ef27fabd0")]
        [AssociationId("f7d98c10-f95f-4852-9973-7c76e9c87afe")]
        [RoleId("50b5f0f0-37ef-40f2-8e66-a9ad17cac8b5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Counter IncomingShipmentNumberCounter { get; set; }

        #region Allors
        [Id("c54b04d9-e38d-410b-9572-a039446fccd0")]
        [AssociationId("f881bbf0-b66e-4690-9a5c-13264351a438")]
        [RoleId("1a950887-6b86-4101-b92d-ab4bb70e4f2d")]
        #endregion
        [Size(256)]
        [Workspace]
        public string QuoteNumberPrefix { get; set; }

        #region Allors
        [Id("5bbfbf49-59ab-49f2-b00f-799cb0568e35")]
        [AssociationId("720f2c78-feec-407f-ae6f-14f9fdac9f2a")]
        [RoleId("4ae74b3f-3530-4eac-8681-dea2344373de")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Counter QuoteNumberCounter { get; set; }

        #region Allors
        [Id("9de2d2aa-c787-4a3c-9e77-49cd60fb2e27")]
        [AssociationId("a9c23b64-a7a1-42f5-8e28-e1faf0b5c861")]
        [RoleId("1970f2a9-4161-4233-a5a7-141ef9282bd8")]
        #endregion
        [Size(256)]
        [Workspace]
        public string WorkEffortNumberPrefix { get; set; }

        #region Allors
        [Id("af0cfafd-6b54-40bd-bdf0-258bf868d202")]
        [AssociationId("f5656a0a-2d86-4432-ad71-647201663b2b")]
        [RoleId("1e5e9eed-5c81-4705-b6e0-f5ae53a29d9a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Counter WorkEffortNumberCounter { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
