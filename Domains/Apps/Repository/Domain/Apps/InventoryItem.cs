namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("61af6d19-e8e4-4b5b-97e8-3610fbc82605")]
    #endregion
	public partial interface InventoryItem : Transitional, UniquelyIdentifiable
    {
    }
}