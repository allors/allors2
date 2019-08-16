namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("d54b4bba-a84c-4826-85ba-7340714035c7")]
    #endregion
    public partial class PostalAddress : ContactMechanism, GeoLocatable, Object
    {
        #region inherited properties
        public string Description { get; set; }

        public ContactMechanism[] FollowTo { get; set; }

        public ContactMechanismType ContactMechanismType { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Allors
        [Id("c83eb0ff-8503-4f2a-9280-f8e46b382b6a")]
        [AssociationId("2976fdd4-19c4-4913-8875-1bf413da02fd")]
        [RoleId("04c32889-628b-4f9f-8504-65a73268055d")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Address1 { get; set; }

        #region Allors
        [Id("9475dd68-43f7-4195-bf57-8ce82333980e")]
        [AssociationId("c0a0b7b4-5f1a-4b8b-a858-5de3abc5e66f")]
        [RoleId("58ffb62f-f270-4a64-b2a9-26b2cec8eaee")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Address2 { get; set; }

        #region Allors
        [Id("5440794c-8569-46fb-a5cb-42dc523e1264")]
        [AssociationId("1f14c608-3744-4697-a226-443196a57e94")]
        [RoleId("db609423-3100-46a1-890d-0dbef16daf3f")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Address3 { get; set; }

        #region Allors
        [Id("24216a78-41d8-4ffc-958a-2411530eeb94")]
        [AssociationId("649eb363-210c-4567-be0a-bcd3e666294e")]
        [RoleId("1e9fb472-c39d-4e46-a58c-3cbf2b99c2cd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public GeographicBoundary[] PostalAddressBoundaries { get; set; }

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
        [Size(256)]
        [Workspace]
        public string Locality { get; set; }

        #region Allors
        [Id("d92c5fd4-68e9-402b-b540-86053df1b70d")]
        [AssociationId("ce1593e7-a08d-43f3-a6af-ea5800ff9d3b")]
        [RoleId("f35bdd80-6821-4d72-8cd7-a8f4d0542fc4")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Region { get; set; }

        #region Allors
        [Id("c0e1c31b-5506-48c0-b46f-239f89eca08f")]
        [AssociationId("09a54b9f-1461-4956-ba7a-fc6f086abf77")]
        [RoleId("226183cc-ae5d-4292-982b-aba15304ab70")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Country Country { get; set; }

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }


        #endregion
    }
}
