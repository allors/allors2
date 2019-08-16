namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("d61872ee-3778-47e8-8931-003f3f48cbc5")]
    #endregion
    public partial interface ValidationI12 : Object
    {
        #region Allors
        [Id("0b89b096-a69a-495c-acfe-b24a9b27e320")]
        [AssociationId("e178ed0f-7764-4836-bd6f-fcb7f5d62346")]
        [RoleId("007a6d25-8506-483d-9140-414c0056d812")]
        #endregion
        Guid UniqueId { get; set; }
    }
}
