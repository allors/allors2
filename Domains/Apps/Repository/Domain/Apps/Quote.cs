namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("066bf242-2710-4a68-8ff6-ce4d7d88a04a")]
    #endregion
	public partial interface Quote : IQuote, Printable, Auditable, Commentable, AccessControlledObject
    {
        #region Allors
        [Id("519F70DC-0C4C-43E7-8929-378D8871CD84")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("39694549-7173-4904-8AE0-DA7390F595A5")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("28349179-5B4F-4B33-B4C4-36FBB6A18EC3")]
        #endregion
        [Workspace]
        void AddNewQuoteItem();
    }
}