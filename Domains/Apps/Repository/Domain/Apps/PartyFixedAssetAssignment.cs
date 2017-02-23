namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("40ee178e-7564-4dfa-ab6f-8bcd4e62b498")]
    #endregion
    public partial class PartyFixedAssetAssignment : AccessControlledObject, Period, Commentable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public string Comment { get; set; }

        #endregion

        #region Allors
        [Id("28afdc0d-ebc7-4f53-b5a1-0cc0eb377887")]
        [AssociationId("8d6a5121-c704-4f04-95de-7e2ab8faecea")]
        [RoleId("e9058932-6beb-4698-89b9-c70e98b30b7f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public FixedAsset FixedAsset { get; set; }
        #region Allors
        [Id("59187015-4689-4ef8-942f-c36ff4c74e64")]
        [AssociationId("4f0c5035-bfd2-4843-8d6e-d3df15a7f5dd")]
        [RoleId("38f3a7f5-53b5-4572-bcb0-347fa3a543f3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Party Party { get; set; }
        #region Allors
        [Id("70c38a47-79c4-4ec8-abfd-3c40ef4239ea")]
        [AssociationId("874b5fdc-a8b9-4b7c-9785-15661917b57a")]
        [RoleId("f243ed6d-eabc-4363-ba37-cf147a166081")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public AssetAssignmentStatus AssetAssignmentStatus { get; set; }
        #region Allors
        [Id("c70f014b-345b-48ad-8075-2a1835a19f57")]
        [AssociationId("95b448b4-4fc5-4bd5-b789-e967de001bbe")]
        [RoleId("aa4aca33-b94c-4527-97db-558fab6805a5")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal AllocatedCost { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}