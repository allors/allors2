namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("fa4303c8-a09d-4dd5-97b3-76459b8e038d")]
    #endregion
    public partial class WorkRequirement : Requirement 
    {
        #region inherited properties
        public DateTime RequiredByDate { get; set; }

        public Party Authorizer { get; set; }

        public RequirementStatus[] RequirementStatuses { get; set; }

        public string Reason { get; set; }

        public Requirement[] Children { get; set; }

        public Party NeededFor { get; set; }

        public Party Originator { get; set; }

        public RequirementObjectState CurrentObjectState { get; set; }

        public RequirementStatus CurrentRequirementStatus { get; set; }

        public Facility Facility { get; set; }

        public Party ServicedBy { get; set; }

        public decimal EstimatedBudget { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("b2d15c8b-a739-4c9d-bc16-eff5e6ca112e")]
        [AssociationId("94c8458e-e890-46b0-bdd4-dbfcb9877ded")]
        [RoleId("22899775-0083-4171-801f-9396c9ba16a3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public FixedAsset FixedAsset { get; set; }
        #region Allors
        [Id("c9b7298e-1a19-4805-94d6-a6a33acccce0")]
        [AssociationId("664c20b0-6cba-43f8-a52a-2655501b9348")]
        [RoleId("9ae7027e-6541-41fd-bae0-6e61c424c864")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Deliverable Deliverable { get; set; }
        #region Allors
        [Id("ef364ba6-62ed-40db-a580-cf7f6f473e27")]
        [AssociationId("beb281bd-199b-4416-bb38-7d21ec376398")]
        [RoleId("8f541554-8bfa-404a-85e9-453f2809d4a4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Product Product { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Reopen(){}

        public void Cancel(){}

        public void Hold(){}

        public void Close(){}



        #endregion

    }
}