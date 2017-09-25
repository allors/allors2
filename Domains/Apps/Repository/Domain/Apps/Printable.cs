namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("61207a42-3199-4249-baa4-9dd11dc0f5b1")]
    #endregion
	public partial interface Printable : AccessControlledObject, UniquelyIdentifiable 
    {


        #region Allors
        [Id("c75d4e4c-d47c-4757-bcb0-25b6daedec9e")]
        [AssociationId("480b7df7-b463-4038-a48d-35b8a8af899e")]
        [RoleId("8d530dcd-2c3b-4d1d-8acc-9963338968ed")]
        #endregion
        [Derived]
        [Size(-1)]

        string PrintContent { get; set; }

    }
}