namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("00cba2fb-feb8-4566-8898-3bde8820211f")]
    #endregion
    public partial class ProductRequirement : Requirement 
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
        [Id("48ce0470-5738-4d9b-ab23-ea244e90091d")]
        [AssociationId("af379058-8ac3-4d0e-8eb4-715fcdda5e44")]
        [RoleId("9237556e-d3c2-4404-a39c-11660471d23d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Product Product { get; set; }
        #region Allors
        [Id("a72274b6-2767-4cb9-8f3d-dc1e367c6f1b")]
        [AssociationId("e991712f-bbed-4cb9-98ef-e7ff2506fc11")]
        [RoleId("57ed8d56-0c40-47a8-9e49-be7a35294800")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public DesiredProductFeature[] DesiredProductFeatures { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

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