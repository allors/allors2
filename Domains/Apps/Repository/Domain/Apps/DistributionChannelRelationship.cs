namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("f278459d-6b7f-47cf-ab0e-05c548faab1e")]
    #endregion
    public partial class DistributionChannelRelationship : PartyRelationship 
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
        [Id("86a07419-5dfd-4618-a472-168ba5fdf3ff")]
        [AssociationId("2800f775-ce61-4684-b6a3-5ce28dcf140b")]
        [RoleId("b61fdf73-2420-498c-af0b-49ecdeec359a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public InternalOrganisation InternalOrganisation { get; set; }
        #region Allors
        [Id("e7c812db-f6c8-431b-9f4d-5317a0d8673c")]
        [AssociationId("21844f4b-372c-45de-acfa-02c428afdbd8")]
        [RoleId("00b349c4-e7f6-4d8f-b4d3-0922a3465a91")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Organisation Distributor { get; set; }


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