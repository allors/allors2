namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("D097D000-BF1B-4974-827E-270A2363CDD0")]
    #endregion
    public partial interface IPurchaseOrderItem : IOrderItem 
    {
        
        #region Allors
        [Id("43035995-bea3-488b-9e81-e85e929faa57")]
        [AssociationId("f9d773a8-772b-4981-a360-944f14a5ef94")]
        [RoleId("f7034bc1-6cc0-4e03-ab3c-da64d427df9b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        PurchaseOrderItemObjectState CurrentObjectState { get; set; }
        
        #region Allors
        [Id("64e30c56-a77d-4ecf-b21e-e480dd5a25d8")]
        [AssociationId("448695c9-c23b-4ae0-98d7-802a8ae4e9f8")]
        [RoleId("9586b58f-8ae0-4b26-81b6-085a9e28aa77")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal QuantityReceived { get; set; }
        
        #region Allors
        [Id("adfe14e7-fbf6-465f-b6e5-1eb3e8583179")]
        [AssociationId("682538a3-d3e7-432b-9264-38197462cee1")]
        [RoleId("fecc85a0-871b-4846-b8f1-c2a5728fbbd2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        Product Product { get; set; }

        #region Allors
        [Id("e2dc0027-220b-4935-bc5a-cb2e2b6be248")]
        [AssociationId("3d24da0d-fdd6-46e3-909b-7710e84e2d68")]
        [RoleId("76ed288c-be72-44e2-8b83-0a0f5a616e52")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        Part Part { get; set; }
    }
}