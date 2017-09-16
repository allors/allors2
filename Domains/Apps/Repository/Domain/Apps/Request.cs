namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("321a6047-2233-4bec-a1b1-9b965c0099e5")]
    #endregion
	public partial interface Request : IRequest, AccessControlledObject, Commentable, Auditable, Printable
    {
        #region Allors
        [Id("B30EDA48-5E99-44AE-B3A9-D053BCFA4895")]
        #endregion
        void Cancel();

        #region Allors
        [Id("2510F8F6-52E1-4024-A0B1-623DFB62395A")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("7458A51F-5EAD-41A0-B44C-A22B4BA2A372")]
        #endregion
        [Workspace]
        void Submit();

        #region Allors
        [Id("0E26CA10-B0D4-47B0-BEDE-F8EC1AE6BD36")]
        #endregion
        [Workspace]
        void Hold();

        #region Allors

        [Id("96F7CC66-A06D-4AF0-B163-038FFD83AED7")]

        #endregion
        [Workspace]
        void AddNewRequestItem();
    }
}