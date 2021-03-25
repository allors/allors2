// <copyright file="IrpfRegime.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c56a909e-ab7b-4b60-abb9-2a9cfec16e43")]
    #endregion
    /// <summary>
    /// Impuesto sobre la renta de las personas fí­sicas.
    /// It is the personal Income Tax in Spain, a direct tax levied on the income of individuals.
    /// if you are selling to a Spanish business you will have to indicate the level of IRPF that should be withheld by the customer.
    /// Essentially this is withheld by your customer and paid to the tax office by them on your behalf.
    /// </summary>
    public partial class IrpfRegime : Enumeration
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("bee6ea26-43c8-44b3-bcbf-68b942c26d1c")]
        [AssociationId("9543ad75-293f-4f02-b0ad-0bbb226c13f2")]
        [RoleId("bb3e7d1e-87a6-4349-9efe-e08571b84632")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public IrpfRate ObsoleteIrpfRate { get; set; }

        #region Allors
        [Id("bd263133-c785-474e-bdc3-a38226cad2ba")]
        [AssociationId("d83a1bf0-2964-4352-a5e8-bdf4d4d3d350")]
        [RoleId("8060e5fd-72c5-4bb8-b787-1d979928a0fe")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        public IrpfRate[] IrpfRates { get; set; }

        /// <summary>
        /// Contains the accounting account assigned for the invoices/credit memos of purchase.
        /// </summary>
        #region Allors
        [Id("a33a4f2c-ad70-4455-8cd6-68606d39446d")]
        [AssociationId("b0840a03-d3e0-4e43-b3a0-b7a60bd54066")]
        [RoleId("65428086-8d61-40d1-8888-02866d0e2512")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public OrganisationGlAccount PurchaseGeneralLedgerAccount { get; set; }

        /// <summary>
        /// contains the accounting account assigned for sales invoices/credit memos.
        /// </summary>
        #region Allors
        [Id("337192f2-085f-4c56-b492-c9c69138c125")]
        [AssociationId("181e688f-203a-4b54-8e68-9e133bbb1ecf")]
        [RoleId("b92fe22a-3d10-43d7-92c3-b5c0c1e52b64")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public OrganisationGlAccount SalesGeneralLedgerAccount { get; set; }

        #region inherited methods

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
