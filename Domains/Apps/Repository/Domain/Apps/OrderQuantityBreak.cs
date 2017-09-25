namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("aa5898e6-71d0-4dcb-9bbd-35ae5cb0e0ef")]
    #endregion
    public partial class OrderQuantityBreak : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("6d20ad83-150b-44d7-940c-725554175ba9")]
        [AssociationId("8d3c682c-a6fa-4ff9-9734-1a0fb21342fe")]
        [RoleId("88caf998-c922-437c-84a2-fa9370c6fb28")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal ThroughAmount { get; set; }
        #region Allors
        [Id("9ac69278-fef8-4f82-8dfa-dcc192274e23")]
        [AssociationId("9a9c5ef7-d3d0-4787-a653-b2c8893bd737")]
        [RoleId("16547884-680e-45fe-a85a-7aa77e896f50")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal FromAmount { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}