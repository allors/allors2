namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5A0B6477-7B54-48FA-AF59-7B664587F197")]
    #endregion
    public partial class ProductCharacteristic : Enumeration
    {

        #region inherited properties

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion


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