namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f35745a9-a8d3-4002-a484-6f0fb00a69a2")]
    #endregion
    public partial class GenderType : Enumeration 
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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

        #region Allors
        [Id("4D572260-2DBB-4371-BF42-209CBA101B47")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}