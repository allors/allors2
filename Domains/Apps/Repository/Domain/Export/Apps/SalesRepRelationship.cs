namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6c28f40a-1826-4110-83c8-7eaefc797f1a")]
    #endregion
    public partial class SalesRepRelationship : Commentable, PartyRelationship 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Party[] Parties { get; set; }

        #endregion

        #region Allors
        [Id("24e8c07a-2fca-496a-8c21-165f29a6733d")]
        [AssociationId("5cd1d447-85b5-4d28-8296-05e356046f62")]
        [RoleId("7d8a933f-1d36-4247-bf13-580aa24bd645")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Person SalesRepresentative { get; set; }

        #region Allors
        [Id("4ffa4073-8359-48c0-8562-9c30e6426ad2")]
        [AssociationId("ba0a1788-bd88-4d93-91c9-a51af7831ba2")]
        [RoleId("6a59c35a-9ffb-4311-b0fa-43ea42b61fd1")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal LastYearsCommission { get; set; }

        #region Allors
        [Id("61a10565-62ac-4529-a3b1-709f3b5da306")]
        [AssociationId("8a3b5d2e-3be7-4c54-9571-d2466f5323ff")]
        [RoleId("6f9f29f3-0f9e-458a-aea6-27dd7e76adfe")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public ProductCategory[] ProductCategories { get; set; }

        #region Allors
        [Id("98dab364-0db6-438f-ac12-9b0238e81afd")]
        [AssociationId("eb32c549-bb3e-4789-abdb-9073905077bb")]
        [RoleId("f213a48e-d351-41c4-91df-48edd0043017")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal YTDCommission { get; set; }

        #region Allors
        [Id("FEC2ABA6-BF9C-4310-BD07-8A40B9EDD646")]
        [AssociationId("333BAF78-24CF-4ABE-82AF-03E8B428ED77")]
        [RoleId("B734240F-6939-4CFC-9ED4-DB2072859C65")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Required]
        public Party Customer { get; set; }

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