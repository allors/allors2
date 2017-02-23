namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("caa2a2de-9454-4812-a69f-9d3728706345")]
    #endregion
    public partial class Image : Deletable 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("366410a7-7d51-4d7c-82fd-3444bdc0b3f7")]
        [AssociationId("9d45e17e-962b-4f9b-a029-c1c1562e5260")]
        [RoleId("9ed94fa8-e01e-4f63-9932-d56134616474")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Media Original { get; set; }
        #region Allors
        [Id("59689164-7a45-45d4-98fa-f8cf50c62899")]
        [AssociationId("386c7cfc-4bec-4564-a7c4-b2c1bccf6ebe")]
        [RoleId("ce4c0fbb-5bdb-4c7f-a70a-b930c1020624")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]

        public Media Responsive { get; set; }
        #region Allors
        [Id("d149b012-1dc2-4bd1-a650-26b7c6f9024b")]
        [AssociationId("75fccc6e-1c89-4e0f-88c2-527eb3b0d71d")]
        [RoleId("2f1c8149-f94a-448b-a832-4994f635c48f")]
        #endregion
        [Size(256)]

        public string OriginalFilename { get; set; }
        #region Allors
        [Id("d54405bf-efa0-4b64-a086-1c85ae0c5b2f")]
        [AssociationId("2af16928-df35-42c6-a468-15e4bae5e035")]
        [RoleId("55f44481-d670-4812-a168-e96509a70e25")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Media Thumbnail { get; set; }

        #region Allors
        [Id("6363CF00-DB85-434F-AA8F-3B6991C79F63")]
        #endregion
        public void CreateResponsive() { }
        
        #region Allors
        [Id("9C99AFA1-D80F-4129-8D95-A18FFE0A68E8")]
        #endregion
        public void CreateThumbnail() { }
        
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