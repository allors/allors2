namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("2b162153-f74d-4f97-b97c-48f04749b216")]
    #endregion
    public partial class SupplierRelationship : PartyRelationship 
    {
        #region inherited properties
        public PartyRelationshipStatus PartyRelationshipStatus { get; set; }

        public Agreement[] RelationshipAgreements { get; set; }

        public Priority PartyRelationshipPriority { get; set; }

        public decimal SimpleMovingAverage { get; set; }

        public CommunicationEvent[] CommunicationEvents { get; set; }

        public Party[] Parties { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public string Comment { get; set; }

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
        [Id("17aa6ceb-0cbd-45fa-9f6d-848ce4a365b1")]
        [AssociationId("b5057208-9823-4a54-8394-6100d18dbe4a")]
        [RoleId("6ddbeb86-39ea-4bd8-ae17-af4b9a1968ce")]
        #endregion
        [Required]
        [Workspace]
        public int SubAccountNumber { get; set; }
        
        #region Allors
        [Id("b12a68f6-0eaa-4a8a-a741-398a0be43f62")]
        [AssociationId("01adc720-91a4-47c6-9235-d21a3215ee6f")]
        [RoleId("0363121b-d92e-4722-81dc-47032aae5440")]
        #endregion
        [Workspace]
        public DateTime LastReminderDate { get; set; }

        #region Allors
        [Id("b1832c65-8b46-4060-bd2a-22c12ff01714")]
        [AssociationId("f320e24c-61b1-4820-a091-d8b63b21184d")]
        [RoleId("4d7752a6-5bf6-420f-b4ed-fcf1cc5952e2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public DunningType DunningType { get; set; }

        #region Allors
        [Id("e96a79e7-c161-4ed9-a5cc-8de4f67bf954")]
        [AssociationId("1a406db2-268a-4669-a629-e0e15fdbd826")]
        [RoleId("aa91e2ad-89c0-411e-9271-56fbf20489f6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InternalOrganisation InternalOrganisation { get; set; }

        #region Allors
        [Id("ee871786-8840-404d-9b41-932a9f59be13")]
        [AssociationId("5b98959d-5589-4958-a86f-4c9b465c1632")]
        [RoleId("056ca61a-1ab4-4e53-8d5f-328ada5f3b11")]
        #endregion
        [Workspace]
        public DateTime BlockedForDunning { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}






        public void Delete(){}
        #endregion

    }
}