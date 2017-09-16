namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("2D5C520D-D385-4220-A6E0-91AD467D838F")]
    #endregion
    public partial interface IProductionRun : IWorkEffort 
    {
        #region Allors
        [Id("108bb811-ece8-42b4-89e2-7a394f848f4d")]
        [AssociationId("8eeef339-d38c-45c4-8300-61bbb33cb205")]
        [RoleId("abbbac9a-74f3-4e7d-a56e-f5ba0c967530")]
        #endregion
        int QuantityProduced { get; set; }

        #region Allors
        [Id("407b8671-79ea-4998-b5ed-188dd4a9f43c")]
        [AssociationId("7358c0de-4918-4998-afb8-ecd122e04e3a")]
        [RoleId("56da6402-ddd9-4bbb-83be-3c368de22d09")]
        #endregion
        int QuantityRejected { get; set; }

        #region Allors
        [Id("558dfd44-26a5-4d64-9317-a121fabaecf1")]
        [AssociationId("69036ddb-1a7b-4bce-8ee2-2610715e47c0")]
        [RoleId("a708d61a-08d1-45db-877b-3eb4514a9069")]
        #endregion
        int QuantityToProduce { get; set; }
    }
}