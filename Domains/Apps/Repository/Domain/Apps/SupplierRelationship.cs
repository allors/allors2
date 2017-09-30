namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("2b162153-f74d-4f97-b97c-48f04749b216")]
    #endregion
    public partial class SupplierRelationship : Period, Deletable, AccessControlledObject
    {
        #region inherited properties
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

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}






        public void Delete(){}
        #endregion
    }
}