namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("71e50a16-fc60-4177-aed0-e89c7f10f465")]
    #endregion
    public partial class Warehouse : Facility 
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