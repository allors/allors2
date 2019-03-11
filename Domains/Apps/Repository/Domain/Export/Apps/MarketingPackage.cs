namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("42adee8e-5994-42e3-afe1-aa3d3089d594")]
    #endregion
    public partial class MarketingPackage : ProductAssociation 
    {
        #region inherited properties
        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("29cb7841-1793-43c3-bcbe-3d69a8e651b5")]
        [AssociationId("49e615c2-afec-4d3a-90d9-eb19840e2bf0")]
        [RoleId("1a8a5695-acad-4cfb-a228-1f05610d56fa")]
        #endregion
        [Size(-1)]

        public string Instruction { get; set; }
        #region Allors
        [Id("70c7d06c-2086-4a60-b2b9-aba2c6f07669")]
        [AssociationId("23ec81db-27f0-4965-bf25-4f0150fd4281")]
        [RoleId("02896b5c-8c38-40d3-89e3-9b3a0d209d3f")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public Product[] ProductsUsedIn { get; set; }
        #region Allors
        [Id("a687e8ff-624c-4794-866f-f4cc653d874c")]
        [AssociationId("7d1b384b-4730-4e61-8a80-8d18ea8e2ae4")]
        [RoleId("ea685b4f-3063-47a9-ba82-980247e903af")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Product Product { get; set; }
        #region Allors
        [Id("ccabc13b-63cc-4cdf-909d-411edc26d648")]
        [AssociationId("31ac20b1-d41d-4aa5-881b-708e38849017")]
        [RoleId("179d1a3b-8325-49da-9009-f104802f189d")]
        #endregion
        [Required]
        [Size(-1)]

        public string Description { get; set; }
        #region Allors
        [Id("dc3c4217-5c42-4ac3-ad16-33f50653bcfc")]
        [AssociationId("82eaf783-4f29-4ede-a285-a7540d0d5f62")]
        [RoleId("fe881a55-eafb-4f83-985f-bb39cff3d2bc")]
        #endregion

        public int QuantityUsed { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}