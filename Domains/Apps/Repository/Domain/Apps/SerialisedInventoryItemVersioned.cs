namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("97377100-9359-4D57-86E3-4B2D8E732F34")]
    #endregion
	public partial interface SerialisedInventoryItemVersioned : InventoryItemVersioned
    {
        #region Allors
        [Id("de9caf09-6ae7-412e-b9bc-19ece66724da")]
        [AssociationId("ba630eb8-3087-43c6-9082-650094a0226e")]
        [RoleId("c0ada954-d86e-46c3-9a99-09209fb812a5")]
        #endregion
        [Required]
        [Unique]
        [Size(256)]
        [Workspace]
        string SerialNumber { get; set; }

        #region Allors
        [Id("887163E8-720C-4CFD-83DC-7B70A2B155E3")]
        [AssociationId("08923E88-696D-490F-82BC-C775156023FD")]
        [RoleId("CE39E19B-1BE5-487A-A728-BBE7BFFD9901")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        SerialisedInventoryItemObjectState CurrentObjectState { get; set; }
    }
}