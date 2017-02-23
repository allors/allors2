namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("f6dceab0-f4a7-435e-abce-ac9f7bd28ae4")]
    #endregion
    public partial class City : GeographicBoundary, AccessControlledObject, CountryBound 
    {
        #region inherited properties
        public string Abbreviation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public Country Country { get; set; }

        #endregion

        #region Allors
        [Id("05ea705c-9800-4442-a684-b8b4251b51ed")]
        [AssociationId("a584625d-889d-4943-a130-fab2697def9f")]
        [RoleId("889ccbe9-96a3-4d8e-9b8c-a1877ab89255")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }
        #region Allors
        [Id("559dd596-e784-4067-a993-b651ac17329d")]
        [AssociationId("06cc0af4-6bb9-4a86-a3e9-496f36002c92")]
        [RoleId("89811da3-093a-42fe-8142-60692f1c3f05")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public State State { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion

    }
}