namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("5cd7ea86-8bc6-4b72-a8f6-788e6453acdc")]
    #endregion
    [Plural("UnitsOfMeasure")]
    public partial interface UnitOfMeasure : UniquelyIdentifiable, AccessControlledObject, Enumeration, Deletable
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
        [Id("9b0e7410-6201-420c-9efc-0689edb33d42")]
        [AssociationId("2e153d1b-3e03-4ff6-84a6-39c9186999f8")]
        [RoleId("60ebb0ec-bcb8-46c5-8293-36b3b0ad3bdb")]
        #endregion
        [Size(256)]
        string Abbreviation { get; set; }
    }
}