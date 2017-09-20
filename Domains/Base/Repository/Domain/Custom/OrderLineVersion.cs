namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region
    [Id("BA589BE8-049B-4107-9E20-FBFEC19477C4")]
    #endregion
    public partial class OrderLineVersion : Version
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid VersionId { get; set; }

        public DateTime VersionTimeStamp { get; set; }

        #endregion

        #region Allors
        [Id("0B9340C2-CE9B-48C7-A476-6D73B8829944")]
        [AssociationId("566324B9-A7B5-4C1D-AC89-2E228C603684")]
        [RoleId("2C78F740-1D90-44BB-AFE6-3360399A1150")]
        #endregion
        public decimal Amount { get; set; }

        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion
    }
}