// <copyright file="ExpenseEntry.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("f15e6b0e-0222-4f9b-8ae2-20c20f3b3673")]
    #endregion
    public partial class ExpenseEntry : ServiceEntry
    {
        #region inherited properties
        public EngagementItem EngagementItem { get; set; }

        public bool IsBillable { get; set; }

        public string Description { get; set; }

        public WorkEffort WorkEffort { get; set; }

        public string Comment { get; set; }

        public Guid DerivationTrigger { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }
        #endregion

        #region Allors
        [Id("0bb04781-d5b4-455c-8880-b5bfbc9d69f8")]
        [AssociationId("cc956cd1-4910-4977-afc5-e76f8bb2dc16")]
        [RoleId("821a410a-afa4-4e6e-b505-3732a864554a")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
