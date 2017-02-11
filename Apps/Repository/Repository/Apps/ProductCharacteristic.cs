namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("5A0B6477-7B54-48FA-AF59-7B664587F197")]
    #endregion
    public partial class ProductCharacteristic : UniquelyIdentifiable, AccessControlledObject
    {

        #region inherited properties

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("31CD485D-510D-4F98-97F6-32561025CD8E")]
        [AssociationId("CEBFE99A-AFE5-4EF7-B6C1-A8BACEE88D90")]
        [RoleId("C3A19721-5EAD-4FD0-8358-291A816DC35B")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        public LocalisedText[] LocalisedCharacteristics { get; set; }

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