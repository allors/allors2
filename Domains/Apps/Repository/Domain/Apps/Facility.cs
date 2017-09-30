namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("cdd79e23-a132-48b0-b88f-a03bd029f49d")]
    #endregion
    [Plural("Facilities")]
    public partial class Facility : AccessControlledObject, GeoLocatable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        #endregion

        #region Allors
        [Id("0E81E9A0-8E7C-4FC9-930A-10D3E67EA17A")]
        [AssociationId("E507D69E-B592-40B0-AF45-3422A4E0A340")]
        [RoleId("9BA88152-C48D-49A6-BFFA-84CD2AE79DB2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public FacilityType FacilityType { get; set; }

        #region Allors
        [Id("1a7f255a-3e94-41df-b71d-10ab36f38ffb")]
        [AssociationId("1341fb5d-26b6-4c07-bb31-a444c451c547")]
        [RoleId("cd2ee41e-ffba-4a59-9b9c-0d3eb581420c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Facility MadeUpOf { get; set; }

        #region Allors
        [Id("1daad895-cf57-4110-a4e0-117e0212c3e4")]
        [AssociationId("304a1b7b-215a-4fad-ab99-d0a974e8b0c0")]
        [RoleId("5ab48116-f33a-484e-9e6e-05a912efc9d5")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal SquareFootage { get; set; }

        #region Allors
        [Id("2df0999d-97cb-4c76-9f3e-076376e60e38")]
        [AssociationId("d29a1df5-e08f-4f7c-876c-a1ab737206a5")]
        [RoleId("5dd1abff-5a4c-4d30-8c69-1bcc83e5460e")]
        #endregion
        [Size(256)]
        public string Description { get; set; }

        #region Allors
        [Id("4b55ee38-64e9-4c11-a204-36e2f460c5f8")]
        [AssociationId("87db14ec-a82a-4107-bae1-8ea945a68bce")]
        [RoleId("d576e2ee-dcf0-4f06-a496-42bceaf94399")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public ContactMechanism[] FacilityContactMechanisms { get; set; }

        #region Allors
        [Id("b8f50794-848b-42be-9114-5eea579f5f71")]
        [AssociationId("7c9d689d-c38d-48b1-b1f0-5b211828ae8a")]
        [RoleId("d9ee92cb-3131-4442-be42-269ae294378d")]
        #endregion
        [Required]
        [Size(256)]
        public string Name { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}