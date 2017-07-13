namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("6c28f40a-1826-4110-83c8-7eaefc797f1a")]
    #endregion
    public partial class SalesRepRelationship : AccessControlledObject, Commentable, Period, PartyRelationship 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public PartyRelationshipStatus PartyRelationshipStatus { get; set; }

        public Agreement[] RelationshipAgreements { get; set; }

        public Priority PartyRelationshipPriority { get; set; }

        public decimal SimpleMovingAverage { get; set; }

        public CommunicationEvent[] CommunicationEvents { get; set; }

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
        [Id("7dc11a0c-72af-4296-94a4-068edae0021a")]
        [AssociationId("8f82d5f9-8f9e-4f57-bb39-4bab9f9813a3")]
        [RoleId("fac65a32-5fcb-4304-9f7d-3ae36da914ff")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InternalOrganisation InternalOrganisation { get; set; }

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
        [Id("b770e679-2da6-45e7-b8e0-2ee39ab67f1e")]
        [AssociationId("95817787-34eb-42f5-82a0-d28bfa93cd88")]
        [RoleId("b86c35e3-b512-4d5e-9f08-29fe8d5a7b43")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
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