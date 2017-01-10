namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("60065f5d-a3c2-4418-880d-1026ab607319")]
    #endregion
    public partial class UserGroup : UniquelyIdentifiable, AccessControlledObject 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("585bb5cf-9ba4-4865-9027-3667185abc4f")]
        [AssociationId("1e2d1e31-ed80-4435-8850-7663d9c5f41d")]
        [RoleId("c552f0b7-95ce-4d45-aaea-56bc8365eee4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public User[] Members { get; set; }
        
        #region Allors
        [Id("e94e7f05-78bd-4291-923f-38f82d00e3f4")]
        [AssociationId("75859e2c-c1a3-4f4c-bb37-4064d0aa81d0")]
        [RoleId("9d3c1eec-bf10-4a79-a37f-bc6a20ff2a79")]
        #endregion
        [Indexed]
        [Required]
        [Unique]
        [Size(256)]
        public string Name { get; set; }
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}