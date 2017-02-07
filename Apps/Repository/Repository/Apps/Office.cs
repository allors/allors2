namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("f444b4be-1703-49b4-918b-2d1aaf27ce5a")]
    #endregion
    public partial class Office : Facility 
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