namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("d79f734d-4434-4710-a7ea-7d6306f3064f")]
    #endregion
	public partial interface InvoiceItem : IInvoiceItem, AccessControlledObject, Transitional, Commentable
    {
    }
}