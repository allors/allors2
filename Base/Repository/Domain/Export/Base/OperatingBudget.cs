namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b5d151c7-0b18-4280-80d1-77b46162dba8")]
    #endregion
    public partial class OperatingBudget : Budget, Versioned
    {
        #region inherited properties


        public string Description { get; set; }

        public BudgetRevision[] BudgetRevisions { get; set; }

        public string BudgetNumber { get; set; }

        public BudgetReview[] BudgetReviews { get; set; }

        public BudgetItem[] BudgetItems { get; set; }

        public BudgetState PreviousBudgetState { get; set; }

        public BudgetState LastBudgetState { get; set; }

        public BudgetState BudgetState { get; set; }

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

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Versioning
        #region Allors
        [Id("4B026D02-D380-44CF-9800-43A496097A75")]
        [AssociationId("8F07ADAF-6BBE-4731-BDB5-BAA35A10436A")]
        [RoleId("A61ED2C0-FBBA-4B81-A466-2C309AABF784")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public OperatingBudgetVersion CurrentVersion { get; set; }

        #region Allors
        [Id("A68F205C-3ED1-49A7-877C-D64761567233")]
        [AssociationId("D166942C-9588-4E3D-9E1B-187F0EB1026B")]
        [RoleId("ABB09E34-8E15-4A19-9373-2522DB729B22")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public OperatingBudgetVersion[] AllVersions { get; set; }
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

        public void Close() { }

        public void Reopen() { }
        #endregion
    }
}
