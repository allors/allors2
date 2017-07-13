namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("e6f97f86-6aec-4dde-b828-4de04d42c248")]
    #endregion
    public partial class County : GeographicBoundary, CityBound, AccessControlledObject 
    {
        #region inherited properties
        public string Abbreviation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public City[] Cities { get; set; }

        #endregion

        #region Allors
        [Id("89a67d5c-8f78-41aa-9152-91f8496535bc")]
        [AssociationId("93664b6a-08d3-48b7-aada-214db9d19cb8")]
        [RoleId("20477c4e-3c7f-4239-a5ae-313465682966")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }
        #region Allors
        [Id("926ce4e6-cc76-4005-964f-f4d5af5fe944")]
        [AssociationId("71bf2977-eb86-4c5d-84f3-7ee97412e460")]
        [RoleId("66743b3b-180e-4a8d-baec-b728fd4ed29c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public State State { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion

    }
}