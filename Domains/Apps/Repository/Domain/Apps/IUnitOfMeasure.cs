namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b7215af5-97d6-42b0-9f6f-c1fccb2bc695")]
    #endregion
	public partial interface IUnitOfMeasure : Enumeration
    {
        #region Allors
        [Id("22d65b11-5d96-4632-9e95-72e30b885942")]
        [AssociationId("873998c2-8c2e-415a-a3c3-6406b21febd8")]
        [RoleId("0543bd39-be9a-49cb-ae23-5df243ee7ea5")]
        #endregion
        [Size(256)]

        string Description { get; set; }


        #region Allors
        [Id("65c75f72-3bb4-415c-8aa7-b291d96dd157")]
        [AssociationId("9225dd82-fdb4-451f-a1cf-000fa37268f1")]
        [RoleId("d202f3f6-2f04-4b2e-8c66-d630be77d76d")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        UnitOfMeasureConversion[] UnitOfMeasureConversions { get; set; }


        #region Allors
        [Id("D35B0EDF-4196-4FE9-8DAA-8B93AEE3B70D")]
        [AssociationId("F3249A01-7C98-4991-B361-078EA6D1DDD8")]
        [RoleId("5530284E-3249-4FD0-979B-C28A2443EF13")]
        #endregion
        string Symbol { get; set; }
    }
}