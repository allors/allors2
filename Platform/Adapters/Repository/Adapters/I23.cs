namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("29cb9717-2452-4da0-9a29-8bd5d815307a")]
    #endregion
	public partial interface I23 :  Object 
    {


        #region Allors
        [Id("0407c93e-f2ea-49e4-8779-44b42c554e60")]
        [AssociationId("9eda27ec-db3f-420a-b9ed-4742b7105bf5")]
        [RoleId("1c1d8356-9240-4582-a3ab-a8a1a2553869")]
        [Size(256)]
        #endregion
        string I23AllorsString { get; set; }

    }
}