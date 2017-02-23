namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("ada24931-020a-48e8-8f8d-18ddb8f46cf7")]
    #endregion
    public partial class Province : CityBound, GeographicBoundary, CountryBound, AccessControlledObject 
    {
        #region inherited properties
        public City[] Cities { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Abbreviation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid UniqueId { get; set; }

        public Country Country { get; set; }

        #endregion

        #region Allors
        [Id("e04bddba-a014-4793-8787-d9cb83ba7d60")]
        [AssociationId("da01d60d-4b8a-4472-9a6a-c21af0963a0b")]
        [RoleId("211c25b7-ecc2-4bdb-a73c-f37090eb165c")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}






        #endregion

    }
}