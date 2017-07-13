namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("f67e7755-5848-4601-ba70-4d1a39abfe4b")]
    #endregion
    public partial class WorkEffortInventoryAssignment : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0bf425d4-7468-4e28-8fda-0b04278cb2cd")]
        [AssociationId("2c6841c6-c161-48e0-a257-d932d99ae7b4")]
        [RoleId("1afed0f6-15fa-4fd2-91f5-648773933e3b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public WorkEffort Assignment { get; set; }
        #region Allors
        [Id("5fcdb553-4b8f-419b-9f12-b9cefa68d39f")]
        [AssociationId("dba27480-4d2f-4e69-af01-4e9afba2cc98")]
        [RoleId("3f7a72a4-2727-4dd6-a602-60ef9b6896af")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public InventoryItem InventoryItem { get; set; }
        #region Allors
        [Id("70121570-c02d-4977-80e4-23e14cbc3fc9")]
        [AssociationId("b4224775-005c-4078-a5b6-2b8a60bc143a")]
        [RoleId("c82f1c25-9c42-4d38-8fae-f8790e2333ef")]
        #endregion

        public int Quantity { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}