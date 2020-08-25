namespace Allors.Repository
{
    using Attributes;
    using System;

    #region Allors
    [Id("ec923a15-6ff4-4ad6-8a7a-eb06a2c2e1b6")]
    #endregion
    public partial class Scoreboard : Object
    {
        #region Inherited Properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        #endregion


        #region Allors

        [Id("8f31ae77-ed28-4931-a689-9261315ad44c")]
        [AssociationId("b6e87c7b-038a-4c37-8845-e3f11f1874fa")]
        [RoleId("4dc3ddba-87b9-4ae7-99e7-8c2e1b2a7f99")]

        #endregion Allors
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public Person[] Players { get; set; }

        #region Allors

        [Id("01ecdeef-5e64-4fdb-969d-b6ef1bd1ed3a")]
        [AssociationId("1acbfb96-e403-4ec3-b9ff-1efa59bd2993")]
        [RoleId("63960f82-6a8c-49ba-8b32-b9d2097cc6e5")]

        #endregion Allors
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Game[] Games { get; set; }

        #region Allors

        [Id("2f11900b-77db-4b8e-a109-958786213f93")]
        [AssociationId("5246e81c-1659-480a-8cae-b5d2540d3d30")]
        [RoleId("d6211162-982f-4a97-8d01-afb3d5e0acfa")]

        #endregion Allors
        [Multiplicity(Multiplicity.OneToMany)]
        [Synced]
        [Workspace]
        public Score[] AccumulatedScores { get; set; }

        #region inherited methods
        public void OnBuild()
        {
            
        }

        public void OnDerive()
        {
            
        }

        public void OnInit()
        {
            
        }

        public void OnPostBuild()
        {
            
        }

        public void OnPostDerive()
        {
            
        }

        public void OnPreDerive()
        {
            
        }

        #endregion inherited methods
    }
}