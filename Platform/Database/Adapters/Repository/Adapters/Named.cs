namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("fcaa52e3-4a90-4981-b45d-d158e2589506")]
    #endregion
	public partial interface Named : Object 
    {


        #region Allors
        [Id("ce43ca5e-4dfb-4fe1-98ea-17d8382e9531")]
        [AssociationId("c7070936-66b1-4f94-af88-40833b35ce37")]
        [RoleId("76de9af1-b334-4e13-ae62-954e6a431ce3")]
        [Size(256)]
        #endregion
        string Name { get; set; }


        #region Allors
        [Id("fdad723a-f062-492a-989c-8d8727c52679")]
        [AssociationId("52a479f2-724b-4e02-9b36-c8c668cb22e6")]
        [RoleId("5a4f9946-44fc-4770-88a4-6141bb1b249e")]
        #endregion
        int Index { get; set; }

    }
}