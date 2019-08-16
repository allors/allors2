namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("62693ee8-1fd3-4b2b-85ce-8d88df3bba0c")]
    #endregion
    public partial class Region : GeographicBoundaryComposite, Object
    {
        #region inherited properties
        public GeographicBoundary[] Associations { get; set; }

        public string Abbreviation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("2b0f6297-9056-4c51-a898-e5bf09e67941")]
        [AssociationId("9e062953-c2a0-44da-b6bf-5669b11fe4ab")]
        [RoleId("e3e9d99b-7780-4528-91dd-d75298bf2437")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }


        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}