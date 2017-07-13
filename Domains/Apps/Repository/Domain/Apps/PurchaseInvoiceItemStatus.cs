namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("6dd3dbf4-d14d-45a3-ad52-65b31a4bb24e")]
    #endregion
    public partial class PurchaseInvoiceItemStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("36ee6713-cf38-4ce1-9173-9d7f8aa75ea4")]
        [AssociationId("7a4e3240-4f48-4c76-ab15-177066f5cbdb")]
        [RoleId("81879f79-834a-4f00-aad5-ddeae84ddf99")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PurchaseInvoiceItemObjectState PurchaseInvoiceItemObjectState { get; set; }
        #region Allors
        [Id("45a00bdb-cbe4-4170-88e2-20bb55dff33f")]
        [AssociationId("5db6dcfb-c4a9-49ea-84f3-f136a184a4fe")]
        [RoleId("e6b78152-6511-4620-81a3-f31709434fc1")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}