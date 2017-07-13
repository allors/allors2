namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4e501cd6-807c-4f10-b60b-acd1d80042cd")]
    #endregion
    public partial class UnitSample: Object, AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("24771d5b-f920-4820-aff7-ea6391b4a45c")]
        [AssociationId("fe3aa333-e011-4a1e-85dc-ded48329cf00")]
        [RoleId("4d4428fc-bac0-47af-ab5e-7c7b87880206")]
        #endregion
        [Workspace]
        public byte[] AllorsBinary { get; set; }

        #region Allors
        [Id("4d6a80f5-0fa7-4867-91f8-37aa92b6707b")]
        [AssociationId("13f88cf7-aaec-48a1-a896-401df84da34b")]
        [RoleId("a462ce40-5885-48c6-b327-7e4c096a99fa")]
        #endregion
        [Workspace]
        public DateTime AllorsDateTime { get; set; }
        
        #region Allors
        [Id("5a788ebe-65e9-4d5e-853a-91bb4addabb5")]
        [AssociationId("7620281d-3d8a-470a-9258-7a6d1b818b46")]
        [RoleId("b5dd13eb-8923-4a66-94df-af5fadb42f1c")]
        #endregion
        [Workspace]
        public bool AllorsBoolean { get; set; }
        
        #region Allors
        [Id("74a35820-ef8c-4373-9447-6215ee8279c0")]
        [AssociationId("e5f7a565-372a-42ed-8da5-ffe6dd599f70")]
        [RoleId("4a95fb0d-6849-499e-a140-6c942fb06f4d")]
        #endregion
        [Workspace]
        public double AllorsDouble { get; set; }
        
        #region Allors
        [Id("b817ba76-876e-44ea-8e5a-51d552d4045e")]
        [AssociationId("80683240-71d5-4329-abd0-87c367b44fec")]
        [RoleId("07070cb0-6e65-4a00-8754-50cf594ed9e1")]
        #endregion
        [Workspace]
        public int AllorsInteger { get; set; }
        
        #region Allors
        [Id("c724c733-972a-411c-aecb-e865c2628a90")]
        [AssociationId("e4917fda-a605-4f6f-8f63-579ec688b629")]
        [RoleId("f27c150a-ce8d-4ff3-9507-ccb0b91aa0c2")]
        #endregion
        [Workspace]
        [Size(256)]
        public string AllorsString { get; set; }
        
        #region Allors
        [Id("ed58ae4c-24e0-4dd1-8b1c-0909df1e0fcd")]
        [AssociationId("f117e164-ce37-4c12-a79e-38cda962adae")]
        [RoleId("25dd4abf-c6da-4739-aed0-8528d1c00b8b")]
        #endregion
        [Workspace]
        public Guid AllorsUnique { get; set; }
        
        #region Allors
        [Id("f746da51-ea2d-4e22-9ecb-46d4dbc1b084")]
        [AssociationId("3936ee9b-3bd6-44de-9340-4047749a6c2c")]
        [RoleId("1408cd42-3125-48c7-86d7-4a5f71e75e25")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AllorsDecimal { get; set; }
        
        #region inherited methods
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}