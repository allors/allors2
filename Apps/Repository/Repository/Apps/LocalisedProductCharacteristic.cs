namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("5A0B6477-7B54-48FA-AF59-7B664587F197")]
    #endregion
    public partial class LocalisedProductCharacteristic : Localised, UniquelyIdentifiable, AccessControlledObject
    {

        #region inherited properties

        public Locale Locale { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("31CD485D-510D-4F98-97F6-32561025CD8E")]
        [AssociationId("CEBFE99A-AFE5-4EF7-B6C1-A8BACEE88D90")]
        [RoleId("C3A19721-5EAD-4FD0-8358-291A816DC35B")]
        #endregion
        [Size(256)]

        public string Characteristic { get; set; }

        #region Allors
        [Id("C426FF0F-455F-4DE3-8E6D-B09E78EE7AA1")]
        [AssociationId("D405F5AE-944A-4F08-862D-32FA2F638D3C")]
        [RoleId("34B66C42-21FE-422E-A5A8-B4D4003F8B3A")]
        #endregion
        [Size(-1)]
        public string Value { get; set; }

        #region inherited methods

        public void OnBuild()
        {
            throw new NotImplementedException();
        }

        public void OnPostBuild()
        {
            throw new NotImplementedException();
        }

        public void OnPreDerive()
        {
            throw new NotImplementedException();
        }

        public void OnDerive()
        {
            throw new NotImplementedException();
        }

        public void OnPostDerive()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}