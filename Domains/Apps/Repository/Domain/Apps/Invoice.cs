namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("a6f4eedb-b0b5-491d-bcc0-09d2bc109e86")]
    #endregion
	public partial interface Invoice : IInvoice, AccessControlledObject, Localised, Commentable, Printable, Auditable, UniquelyIdentifiable 
    {
        #region Allors
        [Id("3D2F6151-C1EC-42B4-AE81-10949F187FE5")]
        #endregion
        [Workspace]
        void AddNewInvoiceItem();
    }
}