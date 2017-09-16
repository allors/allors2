namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7dde949a-6f54-4ece-92b3-d269f50ef9d9")]
    #endregion
    public partial interface Order : IOrder, AccessControlledObject, Printable, UniquelyIdentifiable, Commentable, Localised, Auditable
    {
        #region Allors
        [Id("116D62FC-04E5-407C-B044-7092454C8806")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("F735D397-B989-41E8-A042-5C9EAEB41C32")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("6ECEF1FD-19A6-44E0-97B9-1D0F879074B4")]
        #endregion
        [Workspace]
        void Hold();

        #region Allors
        [Id("4F5D213B-C6FC-424A-B8FE-4493B1D4E7B3")]
        #endregion
        [Workspace]
        void Continue();

        #region Allors
        [Id("213642B9-E825-4BDA-B424-F798FA4E19A9")]
        #endregion
        [Workspace]
        void Confirm();

        #region Allors
        [Id("6167FF6D-DED4-45BC-B4C4-5955B4727200")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("AC3821E2-ED69-4DD5-AF02-FA0E15EEC95D")]
        #endregion
        [Workspace]
        void Complete();

        #region Allors
        [Id("9BD8039C-BDBF-41E5-8AB2-ADF5E139F9C0")]
        #endregion
        [Workspace]
        void Finish();

        #region Allors
        [Id("779BBAD3-46BA-4E99-9F72-45A03764FAD7")]
        #endregion
        [Workspace]
        void AddNewOrderItem();
    }
}