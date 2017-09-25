namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f7e69670-1824-44ea-b2cd-fdef02ef84a7")]
    #endregion
    public partial class TimePeriodUsage : DeploymentUsage 
    {
        #region inherited properties
        public TimeFrequency TimeFrequency { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

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