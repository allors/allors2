namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("8da5bb9b-593b-4c10-91c2-1e9cc2c226d1")]
    #endregion
    public partial class EngineeringDocument : Document 
    {
        #region inherited properties
        public string Name { get; set; }

        public string Description { get; set; }

        public string Text { get; set; }

        public string DocumentLocation { get; set; }

        public string PrintContent { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public string Comment { get; set; }

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