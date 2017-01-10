namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("2361c456-b624-493a-8377-2dd1e697e17a")]
    #endregion
    public partial class ValidationC1 :  Object, ValidationI12 
    {
        #region inherited properties
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