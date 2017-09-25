namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7ba40817-7e42-484e-8272-29a433842054")]
    #endregion
    public partial class PurchaseOrderItemStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("8d0f607b-221b-4f42-8ef8-242e3c35d9ba")]
        [AssociationId("2dbd22c3-b84d-4d65-a176-eecd61da3a89")]
        [RoleId("26ed3c20-bad4-4651-9a6e-d6a27616503e")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("d2fad7ff-945e-4bed-b7a7-39238357eaf3")]
        [AssociationId("db25aff8-16fe-4277-afc2-843d81ace875")]
        [RoleId("2f2af9c6-9ed4-475b-a9fd-c16c12454109")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PurchaseOrderItemObjectState PurchaseOrderItemObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}