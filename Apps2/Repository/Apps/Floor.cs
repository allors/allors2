namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("7c0d1b2d-88bf-41dd-b19d-b6b0ed1cb179")]
    #endregion
    public partial class Floor : Facility 
    {
        #region inherited properties
        public Facility MadeUpOf { get; set; }

        public decimal SquareFootage { get; set; }

        public string Description { get; set; }

        public ContactMechanism[] FacilityContactMechanisms { get; set; }

        public string Name { get; set; }

        public InternalOrganisation Owner { get; set; }

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