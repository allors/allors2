namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("5f16236a-0fa4-4866-9b3d-3951edbd4c81")]
    #endregion
    public partial class Room : Facility, Container 
    {
        #region inherited properties
        public Facility MadeUpOf { get; set; }

        public decimal SquareFootage { get; set; }

        public string Description { get; set; }

        public ContactMechanism[] FacilityContactMechanisms { get; set; }

        public string Name { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid UniqueId { get; set; }

        public Facility Facility { get; set; }

        public string ContainerDescription { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion

    }
}