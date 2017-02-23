namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("b55d1ad5-0ef0-40f0-b8d4-b39c370d7dcf")]
    #endregion
    public partial class Partnership : PartyRelationship 
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
        [Id("c8eafc73-9fb3-4a7b-8349-1dd1e9f64520")]
        [AssociationId("4d6ee3e0-4c0c-4387-b140-e2296c8bcbd4")]
        [RoleId("386770df-4089-482e-9b54-af375c37319f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public InternalOrganisation InternalOrganisation { get; set; }
        #region Allors
        [Id("c9a60b88-e525-4bcd-94bd-3fca8989319f")]
        [AssociationId("309ffb3e-7cd3-4958-9177-e7f25a272579")]
        [RoleId("f77b776b-b957-418b-acfb-a4aad51f7a8a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Organisation Partner { get; set; }


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