namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("e94bf9e1-373d-49e3-a0fe-f21a8b1525d4")]
    #endregion
    public partial class PostalBoundary : AccessControlledObject, Deletable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("2edd7f54-5596-46c1-9f8a-813c947d95fb")]
        [AssociationId("61f54d7a-9ad7-447e-ae79-227833f2473c")]
        [RoleId("68f52b6f-6feb-4e22-ae1a-8ef8334c578f")]
        #endregion
        [Size(256)]
        [Workspace]
        public string PostalCode { get; set; }

        #region Allors
        [Id("7166cc1b-1f00-4cef-9875-8092cd4a76a0")]
        [AssociationId("cb2ca991-e054-44af-b6d1-d860072a0859")]
        [RoleId("dea67366-e6ec-4f64-b450-68c6bae4fec7")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Locality { get; set; }

        #region Allors
        [Id("c0e1c31b-5506-48c0-b46f-239f89eca08f")]
        [AssociationId("09a54b9f-1461-4956-ba7a-fc6f086abf77")]
        [RoleId("226183cc-ae5d-4292-982b-aba15304ab70")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Country Country { get; set; }

        #region Allors
        [Id("d92c5fd4-68e9-402b-b540-86053df1b70d")]
        [AssociationId("ce1593e7-a08d-43f3-a6af-ea5800ff9d3b")]
        [RoleId("f35bdd80-6821-4d72-8cd7-a8f4d0542fc4")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Region { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        public void Delete(){}
        #endregion

    }
}