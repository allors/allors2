namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f3ef0124-e867-4da2-9323-80fbe1f214c2")]
    #endregion
    public partial interface OrderItem : IOrderItem, AccessControlledObject, Commentable, Transitional
    {
        #region Allors
        [Id("5368A2C3-9ADF-46A3-9AC0-9C4A03DEAF9A")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("29D93AE6-FD73-408F-A8F0-CD05D96CF102")]
        #endregion
        [Workspace]
        void Reject();

        #region Allors
        [Id("9C496948-13BA-41C6-B8CB-60323AF3B3E9")]
        #endregion
        [Workspace]
        void Confirm();

        #region Allors
        [Id("DA334EDA-0CD3-4AB4-89C5-41C69D596C7C")]
        #endregion
        [Workspace]
        void Approve();

        #region Allors
        [Id("77ACDB9B-9A55-44C6-AAC4-99ACE0924EDB")]
        #endregion
        [Workspace]
        void Finish();

        #region Allors
        [Id("CBA21197-F595-4526-8DB3-43382CCD08E4")]
        #endregion
        [Workspace]
        void Delete();
    }
}