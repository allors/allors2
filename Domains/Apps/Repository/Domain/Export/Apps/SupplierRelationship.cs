namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("2b162153-f74d-4f97-b97c-48f04749b216")]
    #endregion
    public partial class SupplierRelationship : PartyRelationship, Period, Deletable, AccessControlledObject
    {
        #region inherited properties

        public Party[] Parties { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("1546c9f0-84ce-4795-bcea-634d6a78e867")]
        [AssociationId("56c5ff64-f67b-4830-a1e4-11661b0ff898")]
        [RoleId("a0e757a2-d780-43a1-8b21-ab2fc4d75e7e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Organisation Supplier { get; set; }

        #region Allors
        [Id("70914837-D4C3-472B-A6DA-E8EE42D36E99")]
        [AssociationId("24423780-B007-4188-8019-6D74C511D639")]
        [RoleId("5BA1B394-A12F-4D0B-BA8A-AB7C701C5B03")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation InternalOrganisation { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        public void Delete(){}
        #endregion
    }
}