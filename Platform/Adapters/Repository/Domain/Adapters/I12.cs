namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("97755724-b934-4cc5-beb4-3d49a7a4b27e")]
    #endregion
    public partial interface I12 : Object, S12 
    {
        #region Allors
        [Id("1a0eb6ea-d877-42c9-a35a-889fb347f883")]
        [AssociationId("4a18e24c-e031-49e4-b77a-51ebdc29952b")]
        [RoleId("cf993b81-671e-44f9-b7fc-96a4f0ac8522")]
        #endregion
        bool I12AllorsBoolean { get; set; }


        #region Allors
        [Id("249ff221-9261-4219-b0a8-0dc2a8dac8db")]
        [AssociationId("7b0b63ac-66e1-4192-b2a4-7f49be11cb92")]
        [RoleId("d42bc583-ec2c-4154-9697-132e96e38030")]
        #endregion
        int I12AllorsInteger { get; set; }


        #region Allors
        [Id("2c05b90e-a036-450a-8b4e-ee70c8146fed")]
        [AssociationId("886f5fe7-29ea-41ff-a982-8e6763ba2d04")]
        [RoleId("99086a40-8ca7-4b26-a067-9801223c9bc3")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        I34[] I12I34one2manies { get; set; }


        #region Allors
        [Id("3327e14d-5601-4806-b6c5-b740a2c3aa38")]
        [AssociationId("e400d752-2b35-4b8f-b4cb-12fc6c9ba4ab")]
        [RoleId("de5b73d9-3ea5-461a-8c82-e22c314e23e4")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        C3 C3many2one { get; set; }


        #region Allors
        [Id("3589d5bc-3338-449a-bd14-34a19d92251e")]
        [AssociationId("72892ef3-e425-400b-af3d-9ebde3d15747")]
        [RoleId("c145f01d-f08a-461a-a323-ce20caf59cc5")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        C2 I12C2many2one { get; set; }


        #region Allors
        [Id("4c7dd6a2-db16-4477-9b21-34dcb8f50738")]
        [AssociationId("24538f44-5ea8-49f8-a67f-55b585acdcb4")]
        [RoleId("ee7dfee2-bc14-498e-b9e5-6d25bb5fafb1")]
        #endregion
        double I12AllorsDouble { get; set; }


        #region Allors
        [Id("61fc731f-d769-4eb9-bf87-983e73e403e4")]
        [AssociationId("fa9c5986-648a-4c6d-867b-4d5089885b76")]
        [RoleId("ca503649-1e95-42aa-8bb7-6d6cd1708ba8")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        I34 I12I34many2one { get; set; }


        #region Allors
        [Id("716d13fc-f608-41a8-ac9e-824890c585b5")]
        [AssociationId("14da4483-1238-414d-8e2a-4a61d2730b82")]
        [RoleId("9d9840b6-ac0d-460a-8ae4-974e86bce32d")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        I34[] I12I34many2manies { get; set; }


        #region Allors
        [Id("74a22498-ec2c-441b-a42c-0c248ace685d")]
        [AssociationId("41fe122c-2f8a-4b1c-bb56-9d918ecfc05c")]
        [RoleId("4d857d4a-5ba1-4bcb-81d4-06ec0093bb98")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        C3 I12C3one2one { get; set; }


        #region Allors
        [Id("7f373030-657a-4c6b-a086-ac4de33e4648")]
        [AssociationId("f71c20be-c628-4ce2-b4a1-0c0519c7488b")]
        [RoleId("b932ffb4-70ad-4f1a-a497-dffa8b165b10")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        C2[] I12C2many2manies { get; set; }


        #region Allors
        [Id("9fbca845-1f98-4ac8-8117-fa66bbe287eb")]
        [AssociationId("ec0a4183-87a3-4755-b9f1-d887bf966605")]
        [RoleId("5495d54b-111e-43a4-8fcf-0b3af218340c")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal I12AllorsDecimal { get; set; }


        #region Allors
        [Id("afabb84c-f1b3-423b-9028-2ec5bb58e994")]
        [AssociationId("08c02665-ae9b-4662-848b-de19a5285a69")]
        [RoleId("9333b3f8-9c12-4627-9196-b4b123b530b2")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        C2 I12C2one2one { get; set; }


        #region Allors
        [Id("b0fc73fb-fa74-4e8c-b9e1-17c01698f342")]
        [AssociationId("9fb686d0-2bad-4bba-be8f-0dab7d6b0106")]
        [RoleId("c584f1da-431f-4d9c-8870-aad87e4f104b")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        C3[] I12C3one2manies { get; set; }


        #region Allors
        [Id("b889bc75-3d93-4577-a4d7-752393284220")]
        [AssociationId("7df93147-276f-419d-b4b2-ff6dba76c683")]
        [RoleId("18a37e3c-539c-4cf5-9f75-05295f1bacda")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        C3[] I12C3many2manies { get; set; }


        #region Allors
        [Id("c2d1f044-b996-4b16-8fe3-1786f86973b1")]
        [AssociationId("802b30da-76ab-4518-b6d8-204366a26b5e")]
        [RoleId("decf4aeb-6d1f-4f6d-b130-ba706a388326")]
        [Size(256)]
        #endregion
        string PrefetchTest { get; set; }


        #region Allors
        [Id("c3a2e1da-307c-4fad-ab34-6e9d07eea74f")]
        [AssociationId("227fc236-366e-41bd-b694-5bc4a98c2b48")]
        [RoleId("48afb12e-a36e-43f1-b1f1-c8b4e36de8b8")]
        #endregion
        DateTime I12AllorsDateTime { get; set; }


        #region Allors
        [Id("e227ff6c-a4df-49cf-a02f-04e94af6eb4b")]
        [AssociationId("39578e21-cafc-4862-a5ad-3298f2472b3b")]
        [RoleId("692c22ff-7c88-463b-9399-966ea41228a6")]
        [Size(256)]
        #endregion
        string I12AllorsString { get; set; }


        #region Allors
        [Id("f31ace17-76b1-46db-9fc0-099b94fbada5")]
        [AssociationId("a44d261b-bda9-445f-b811-420bc411de51")]
        [RoleId("e7c49ccf-53d1-468c-9f2a-fe31cd1d2a98")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        I34 I12I34one2one { get; set; }


        #region Allors
        [Id("f37b107e-74e5-401f-a7e8-8ac54ceb6c73")]
        [AssociationId("fca41bdf-2372-4115-aa46-b91b98b728b9")]
        [RoleId("230b5ca6-7f6d-4153-b55a-b2d1a9d2b7da")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        C2[] I12C2one2manies { get; set; }

    }
}