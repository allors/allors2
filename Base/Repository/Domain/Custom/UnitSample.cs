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


        #region Allors
        [Id("6E05C521-B90A-459E-931A-940B4D769C6A")]
        [AssociationId("A3EBF97C-B23A-46C5-AA34-AC81F97089A4")]
        [RoleId("EF528E3E-065C-4762-AB4A-637B285A89EB")]
        #endregion
        [Required]
        public byte[] RequiredBinary { get; set; }

        #region Allors
        [Id("0A17B766-9A60-4061-8FCB-AADFC6C13FAF")]
        [AssociationId("07516E8D-E5D2-4975-82B2-94BD419F061D")]
        [RoleId("6E4AA664-3F19-46C1-BA3E-C220E62A9800")]
        #endregion
        [Required]
        public DateTime RequiredDateTime { get; set; }

        #region Allors
        [Id("22BEF3E8-1178-4717-9BD1-D6F34569B63C")]
        [AssociationId("B6698E9B-E371-4906-97F5-C44E18155FDA")]
        [RoleId("520E7D24-AEFB-4FE9-BE12-69823E2F1C37")]
        #endregion
        [Required]
        public bool RequiredBoolean { get; set; }

        #region Allors
        [Id("FAC655F6-6D89-4CE5-B8E9-388F35294DD0")]
        [AssociationId("EA8C33BC-450F-4DB3-A76C-CEC9AEF751CB")]
        [RoleId("4D837324-8433-491E-9E0C-85959EE087F7")]
        #endregion
        [Required]
        public double RequiredDouble { get; set; }

        #region Allors
        [Id("3257637E-CE68-49B8-879C-E428810DD316")]
        [AssociationId("9AD8FC7A-8645-4D61-AC8B-27B048BB920F")]
        [RoleId("5FA9588F-D201-4A85-9A8F-708095D96F1A")]
        #endregion
        [Required]
        public int RequiredInteger { get; set; }

        #region Allors
        [Id("38405B1B-8469-47D9-BDDF-66B753F52A52")]
        [AssociationId("52463620-5352-4577-97B9-07A662FB0D10")]
        [RoleId("34A1060C-BC52-4051-ACB5-BFF3A55C8300")]
        #endregion
        [Required]
        public string RequiredString { get; set; }

        #region Allors
        [Id("336175A6-29FE-4A6A-A21E-5F3B97BFF99D")]
        [AssociationId("D9E3E7DE-07DB-4243-9311-4220DB6E767B")]
        [RoleId("59D7AE57-E7D9-4921-97F5-A1BD02A7E632")]
        #endregion
        [Required]
        public Guid RequiredUnique { get; set; }

        #region Allors
        [Id("A5905304-6BB6-4B15-85F7-8C4225D6E6B9")]
        [AssociationId("DC6E60C0-B3D2-43C2-A6BF-222D1652D6D5")]
        [RoleId("B87C5F55-5F37-4709-9214-571A2E4C2BC2")]
        #endregion
        [Required]
        public decimal RequiredDecimal { get; set; }

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