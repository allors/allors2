namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("07E8F845-5ECC-4B42-83EF-BB86E6B10A69")]
    #endregion
    public partial class PaymentState : Object, ObjectState 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize()
        {
            throw new NotImplementedException();
        }

        public void OnFinalize()
        {
            throw new NotImplementedException();
        }

        public void OnPostFinalize()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}