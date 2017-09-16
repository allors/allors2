namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("E48AB0E0-6999-4C84-94F8-74E6FDE71BAE")]
    #endregion
    public partial interface IPurchaseInvoiceItem : IInvoiceItem 
    {
        #region Allors
        [Id("56e47122-faaa-4211-806c-1c19695fe434")]
        [AssociationId("826db2b1-3048-4237-8e83-0c472a166d49")]
        [RoleId("893de8bc-93eb-4864-89ba-efdb66b32fd5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        PurchaseInvoiceItemType PurchaseInvoiceItemType { get; set; }

        #region Allors
        [Id("65eebcc4-d5ef-4933-8640-973b67c65127")]
        [AssociationId("40703e06-25f8-425d-aa95-3c73fafbfa81")]
        [RoleId("05f86785-08d8-4282-9734-6230e807181b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        Part Part { get; set; }

        #region Allors
        [Id("dbe5c72f-63e0-47a5-a5f5-f8a3ff83fd57")]
        [AssociationId("f8082d94-30fa-4a58-8bb0-bc5bb4f045ef")]
        [RoleId("69360188-077f-49f0-ba88-abb1f546d72c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        PurchaseInvoiceItemObjectState CurrentObjectState { get; set; }
    }
}