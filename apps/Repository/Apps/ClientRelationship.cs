namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("aadaf02e-0bb8-4862-a354-488f39aa8f4e")]
    #endregion
    public partial class ClientRelationship : PartyRelationship 
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
        [Id("d611f21a-1045-40ea-b05b-0c29913d5f1c")]
        [AssociationId("115baf34-414a-4cfa-8d1f-dfbeb7264077")]
        [RoleId("69161130-4a2e-430e-92a2-b8ab0e6ef8dc")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Party Client { get; set; }
        #region Allors
        [Id("e081884c-3db2-4be3-9c85-9167528d751b")]
        [AssociationId("32544879-3730-449a-9835-8decbfe9f4fc")]
        [RoleId("2f9c92b5-7cf2-42ba-924d-4b5d0c73956c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public InternalOrganisation InternalOrganisation { get; set; }


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