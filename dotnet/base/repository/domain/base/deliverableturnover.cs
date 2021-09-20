// <copyright file="DeliverableTurnover.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("48733d8e-506a-4add-a230-907221ca7a9a")]
    #endregion
    public partial class DeliverableTurnover : ServiceEntry
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
        [Id("5c9b7809-0cb0-4282-ae2b-20407126384d")]
        [AssociationId("8e050223-57c1-47b2-b5b4-bdb93840f527")]
        [RoleId("8d3abfcb-f4de-4d6b-9427-b7906430a178")]
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
