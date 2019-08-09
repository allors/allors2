namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6ea1f500-13a2-4f5a-8026-a1d5a57170ac")]
    #endregion
    public partial class CaseState : ObjectState 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] ObjectDeniedPermissions { get; set; }

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

        #endregion

    }
}