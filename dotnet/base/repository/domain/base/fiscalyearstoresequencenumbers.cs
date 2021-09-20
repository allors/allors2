// <copyright file="FiscalYearStoreSequenceNumbers.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("341fa885-0161-406b-89e6-08b1c92cd3b3")]
    #endregion
    [Plural("FiscalYearsStoreSequenceNumbers")]

    public partial class FiscalYearStoreSequenceNumbers : Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("c1b0dcb6-8627-4a47-86d0-2866344da3f1")]
        [AssociationId("3d1c515f-a52f-4038-9820-794f44927beb")]
        [RoleId("ba7329de-0176-4782-92e1-1cd932823ec0")]
        #endregion
        [Required]
        public int FiscalYear { get; set; }

        #region Allors
        [Id("e5c02d81-e0de-412b-91ad-5da3342a749c")]
        [AssociationId("1032a7af-ce21-4b49-9e51-39c6753bbdc1")]
        [RoleId("cedd2415-1068-4abc-b5ef-c56ac0e50cd8")]
        #endregion
        [Size(256)]
        [Workspace]
        public string SalesInvoiceNumberPrefix { get; set; }

        #region Allors
        [Id("14f064a8-461c-4726-93c4-91bc34c9c443")]
        [AssociationId("02716f0b-8fef-4791-85ae-7c15a5581433")]
        [RoleId("5377c7e0-8bc0-4621-83c8-0829c3fae3f2")]
        #endregion
        [Derived]
        [Required]
        public int ObsoleteNextSalesInvoiceNumber { get; set; }

        #region Allors
        [Id("c21f533a-cf7a-4c15-8af4-40d87d4ad162")]
        [AssociationId("3dfa99d4-7123-4431-b315-c57325b018c0")]
        [RoleId("84d6acfa-d071-46ff-a541-e6225c79674c")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        public Counter SalesInvoiceNumberCounter { get; set; }

        #region Allors
        [Id("fe09d5c7-c750-47e1-8430-ee57bc30ae40")]
        [AssociationId("84060cfc-2616-4016-be6e-f3528b3b1f93")]
        [RoleId("bb21d985-f931-4e54-9598-2c698c0d2d13")]
        #endregion
        [Size(256)]
        [Workspace]
        public string CreditNoteNumberPrefix { get; set; }

        #region Allors
        [Id("C349F8A9-82D8-406B-B026-AFBE67DCD375")]
        [AssociationId("6D7DF8E3-43C0-439A-B1D7-ECB3B1E367D0")]
        [RoleId("07C36822-3147-47AC-8B68-542E66038FB9")]
        #endregion
        [Derived]
        [Required]
        public int ObsoleteNextCreditNoteNumber { get; set; }

        #region Allors
        [Id("a1f93578-bde7-427b-abf2-6eaffb9d9e99")]
        [AssociationId("43079efb-996a-48ec-84ed-6da4f3a2e896")]
        [RoleId("ab2c877e-e21a-46bb-a05d-7a6f0a2b9d22")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        public Counter CreditNoteNumberCounter { get; set; }

        #region Allors
        [Id("74fd1e41-11c6-4d40-9108-554314b53d12")]
        [AssociationId("2e56fd88-f5cb-4236-920b-fb560d9e6d86")]
        [RoleId("56ac1590-9bbf-4f1b-9f3b-c0d3375c1a55")]
        #endregion
        [Size(256)]
        [Workspace]
        public string OutgoingShipmentNumberPrefix { get; set; }

        #region Allors
        [Id("2fe9d593-f56f-4be0-87a3-72b0c02862dc")]
        [AssociationId("d5cc5e38-6473-4648-8bb6-7286c8f2a895")]
        [RoleId("6e3f1fd9-e25a-4f43-b6c8-fcfe9e6a41cb")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        public Counter OutgoingShipmentNumberCounter { get; set; }

        #region Allors
        [Id("4657bfc5-c276-44fa-a8ac-dbff2d046701")]
        [AssociationId("f9bc4b5d-e8a1-4490-a9c6-08595b2481b6")]
        [RoleId("c74267fe-a18c-416e-80ae-7cc4b431307f")]
        #endregion
        [Size(256)]
        [Workspace]
        public string SalesOrderNumberPrefix { get; set; }

        #region Allors
        [Id("6c1c7ee3-a14c-46aa-9b92-6463485c53f5")]
        [AssociationId("0fd9a662-682d-4d73-b2da-ee3e20c20f20")]
        [RoleId("aa227f01-d469-41f9-a61a-ab5c322dd69c")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        public Counter SalesOrderNumberCounter { get; set; }

        #region inherited methods

        public Restriction[] Restrictions { get; set; }

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
