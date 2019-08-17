// <copyright file="Budget.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("ebd4da8c-b86a-4317-86b9-e90a02994dcc")]
    #endregion
    public partial interface Budget : Period, Commentable, UniquelyIdentifiable, Transitional
    {
        #region ObjectStates
        #region BudgetState
        #region Allors
        [Id("2DD77672-178C-4561-804F-DB95A24D4DB4")]
        [AssociationId("A72A8A7B-9EF4-4BC7-AA56-D5CFBB952A63")]
        [RoleId("F2F42BBB-BCA0-4B29-83E9-D531FF70B332")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        BudgetState PreviousBudgetState { get; set; }

        #region Allors
        [Id("7F959434-3302-4F06-B008-D80C356AD271")]
        [AssociationId("F7B72F3A-5EFC-44A3-AC31-00CDCD55F2C7")]
        [RoleId("A8C0278A-5965-4BF8-A62F-1A528C3A98C5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        BudgetState LastBudgetState { get; set; }

        #region Allors
        [Id("C9030E23-C2E0-4C2C-8484-6FB8F5C1BFE1")]
        [AssociationId("89FBED90-A741-4F17-ADF5-B7DC807D8D37")]
        [RoleId("F10B5459-9FDD-4596-906F-2B558E38021A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        BudgetState BudgetState { get; set; }
        #endregion
        #endregion

        #region Allors
        [Id("1848add9-ab90-4191-b7f1-eb392be3ec4e")]
        [AssociationId("8232c215-e592-4ec7-8c44-391c917b7e89")]
        [RoleId("5e27d83d-a601-4101-b4dd-7eef98de82e8")]
        #endregion
        [Required]
        [Size(-1)]
        string Description { get; set; }

        #region Allors
        [Id("1c3dd3b4-b514-4a42-965f-d3200325d78c")]
        [AssociationId("dccc1ed1-0cac-4e25-a7ee-5848af5b390e")]
        [RoleId("684c491e-c764-4d83-a11f-d3cf80d671ad")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        BudgetRevision[] BudgetRevisions { get; set; }

        #region Allors
        [Id("494d04ef-aafc-4482-a5c2-4ec9fa93d158")]
        [AssociationId("eda25f81-bba9-4e23-9074-4e22338ace23")]
        [RoleId("d2a2990a-2966-4302-8c18-0884915f9d33")]
        #endregion
        [Size(256)]
        string BudgetNumber { get; set; }

        #region Allors
        [Id("834432b1-65b2-4499-a83d-71f0db6e177b")]
        [AssociationId("b7f09631-6b4c-417d-ba12-115d07d9d6f5")]
        [RoleId("b9ba1402-ce06-4bdd-9290-165ff8e555d2")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        BudgetReview[] BudgetReviews { get; set; }

        #region Allors
        [Id("f6078f5b-036f-45de-ab4f-fb26b6939d11")]
        [AssociationId("ba8edec9-a429-482d-bfbd-4f7fd419eaf7")]
        [RoleId("9b9e4779-bb7d-4edb-b432-eab76472135a")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        BudgetItem[] BudgetItems { get; set; }

        #region Allors
        [Id("A6ED3503-571A-4800-B1BE-379CE197584F")]
        #endregion
        void Close();

        #region Allors
        [Id("B33D83BD-D1E5-4544-9B18-999EF78E4AE2")]
        #endregion
        void Reopen();
    }
}
