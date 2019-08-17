// <copyright file="CapitalBudget.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("41f1aa5a-5043-42bb-aaf5-7d57a9deaccb")]
    #endregion
    public partial class CapitalBudget : Budget, Versioned
    {
        #region inherited properties

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public BudgetRevision[] BudgetRevisions { get; set; }

        public string BudgetNumber { get; set; }

        public BudgetReview[] BudgetReviews { get; set; }

        public BudgetItem[] BudgetItems { get; set; }

        public BudgetState PreviousBudgetState { get; set; }

        public BudgetState LastBudgetState { get; set; }

        public BudgetState BudgetState { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Versioning
        #region Allors
        [Id("7DDE0157-1112-4CF4-ADB4-5D8293DED7C8")]
        [AssociationId("3AA9D970-E071-4C15-B311-BA7663F8A8A3")]
        [RoleId("5128E7AA-F575-4508-94AA-2E9C3333AB84")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderVersion CurrentVersion { get; set; }

        #region Allors
        [Id("31B6B215-5115-4B2C-A5A9-11031A38D533")]
        [AssociationId("118922D3-0D09-48D4-B6FA-E4471F857AFC")]
        [RoleId("61E37EDB-4378-49FE-AE9D-B7E18C38B2C3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public string Description { get; set; }

        public void Close() { }

        public void Reopen() { }
        #endregion
    }
}
