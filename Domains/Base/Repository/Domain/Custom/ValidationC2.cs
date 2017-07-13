namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c7563dd3-77b2-43ff-92f9-a4f98db36acf")]
    #endregion
    public partial class ValidationC2 : Object, ValidationI12 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        #endregion

        #region inherited methods
        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}