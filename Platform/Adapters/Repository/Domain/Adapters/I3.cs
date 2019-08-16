namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("2d86277f-3993-4831-a7de-3640166d3d50")]
    #endregion
    public partial interface I3 : Object, S1234, S3
    {
        #region Allors
        [Id("00b706bb-681e-44ce-bbf3-c3b01bb11269")]
        [AssociationId("e1b2e665-2459-4864-a2ef-bfbb6b17e59c")]
        [RoleId("6f15395d-0754-45b2-82d3-bb172c716b67")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        C4[] I3C4many2manies { get; set; }


        #region Allors
        [Id("25a3bcbf-cd9a-4735-879d-c5415b19cf88")]
        [AssociationId("fb67b815-0a57-4e62-ab96-7980bd6e5c64")]
        [RoleId("f0ee9b2a-f757-40bf-a300-3d307cdfe671")]
        [Size(256)]
        #endregion
        string I3AllorsString { get; set; }


        #region Allors
        [Id("2b273c39-cc85-4585-806f-d991f43dda29")]
        [AssociationId("b6a50007-d627-4d8e-aa22-d683581b8b79")]
        [RoleId("fbd1c026-ed93-4ba5-905b-20047e891445")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        I4[] I3I4one2manies { get; set; }


        #region Allors
        [Id("3a55d57f-768f-4c11-9c18-baa5f3eeda8c")]
        [AssociationId("5024f9c4-f1e7-43e6-b228-4a36b0f2377a")]
        [RoleId("7d3716d2-a9a7-484a-84d4-5e3367c745bb")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        C4[] I3C4one2manies { get; set; }


        #region Allors
        [Id("3f553db3-b490-4de5-b388-5d096d83de0d")]
        [AssociationId("8d7b37bb-8524-4930-a33b-d14b5bdf126b")]
        [RoleId("551eb2b5-a9b1-4e82-b74e-187f2b90fd09")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        I4[] I3I4many2manies { get; set; }


        #region Allors
        [Id("57f8f305-e1a9-452b-bcc1-febf7ccc346a")]
        [AssociationId("9f17390d-c9cb-4241-adb5-b363a1d8d0de")]
        [RoleId("2549ff17-6eb6-4d93-8d22-78aa3c4394b3")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        I4 I3I4many2one { get; set; }


        #region Allors
        [Id("cc48853e-46f3-4292-be9b-8a4937cea308")]
        [AssociationId("08ef8f7c-a9eb-4f7f-be48-7b286d4efff3")]
        [RoleId("5c9157ec-adf2-4575-9b83-daf923811fcd")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        C4 I3C4one2one { get; set; }


        #region Allors
        [Id("d36e7cf1-08d1-4333-b539-e50503c10934")]
        [AssociationId("44136f72-586f-47c0-a84b-1505ff2723e2")]
        [RoleId("f10c8012-b731-470b-af7a-28654e2d572e")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        I4 I3I4one2one { get; set; }


        #region Allors
        [Id("d5ff5333-6bbc-4bb5-8208-44e1d4b53aee")]
        [AssociationId("683c30c4-22d4-4837-8e6f-f503351938ab")]
        [RoleId("5d06be1b-e9a8-4793-afa0-63633d0552ed")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        C4 I3C4many2one { get; set; }


        #region Allors
        [Id("e0cf6092-d865-4386-823b-a2906a3eab1a")]
        [AssociationId("4a1758c9-4cbe-4509-b0dd-da644ad61f15")]
        [RoleId("7f3eee6c-dbd7-4287-9ba7-d0f3289ad0c6")]
        [Size(256)]
        #endregion
        string I3StringEquals { get; set; }


        #region Allors
        [Id("fb90c539-a392-4618-bb0b-9809a3a673aa")]
        [AssociationId("cc3a7bd2-b6c0-4693-a68d-87b6627128d7")]
        [RoleId("10b62caf-0cc3-40cb-a81a-e8aedd50a8e4")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        C1 C1one2one { get; set; }

    }
}