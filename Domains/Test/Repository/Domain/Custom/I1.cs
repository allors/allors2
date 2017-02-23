namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("fefcf1b6-ac8f-47b0-bed5-939207a2833e")]
    #endregion
	public partial interface I1 :  Object, I12, S1 
    {


        #region Allors
        [Id("06b72534-49a8-4f6d-a991-bc4aaf6f939f")]
        [AssociationId("854c6ec4-51d4-4d68-bd26-4168c26707de")]
        [RoleId("9fd09ce4-3f52-4889-b018-fd9374656e8c")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        I1 I1I1Many2One { get; set; }


        #region Allors
        [Id("0a2895ec-0102-493d-9b94-e12e94b4a403")]
        [AssociationId("295bfc0e-e123-4ac8-84da-45e8d77b1865")]
        [RoleId("94c8ca3f-45d6-4f70-8b4a-5d469b0ee897")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        I12[] I1I12Many2Manies { get; set; }


        #region Allors
        [Id("0acbea28-f8aa-477c-b296-b8976d9b10a5")]
        [AssociationId("5b4da68a-6aeb-4d5c-8e09-5bef3b1358a9")]
        [RoleId("5e8608ed-7987-40d0-a877-a244d6520554")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        I2[] I1I2Many2Manies { get; set; }


        #region Allors
        [Id("194580f4-e0e3-4b52-b9ba-6020171be4e9")]
        [AssociationId("39a81eb4-e1bb-45ef-8126-21cf233ba684")]
        [RoleId("98017570-bc3b-442b-9e51-b16565fa443c")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        I2 I1I2Many2One { get; set; }


        #region Allors
        [Id("28ceffc2-c776-4a0a-9825-a6d1bcb265dc")]
        [AssociationId("0287a603-59e5-4241-8b2e-a21698476e67")]
        [RoleId("fec573a7-5ab3-4f30-9b50-7d720b4af4b4")]
        [Size(256)]
        [Workspace]
        #endregion
        string I1AllorsString { get; set; }


        #region Allors
        [Id("2e85d74a-8d13-4bc0-ae4f-42b305e79373")]
        [AssociationId("d6ccfcb8-623e-4852-a878-d7cb377af853")]
        [RoleId("ec030f88-1060-4c2b-bda1-d9c5dc4fc9d3")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        I12 I1I12Many2One { get; set; }


        #region Allors
        [Id("32fc21cc-4be7-4a0e-ac71-df135be95e68")]
        [AssociationId("e0006bdc-74e2-4067-871c-6f0b53eba5de")]
        [RoleId("12824c37-d0d2-4cb9-9481-cad7f5f54976")]
        #endregion
        DateTime I1AllorsDateTime { get; set; }


        #region Allors
        [Id("39e28141-fd6b-4f49-8884-d5400f6c57ff")]
        [AssociationId("9118c09c-e8c2-4685-a464-9be9ece2e746")]
        [RoleId("a4b456e2-b45f-4398-875b-4ba99ead49fe")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        I2[] I1I2One2Manies { get; set; }


        #region Allors
        [Id("4506a14b-22f1-41fe-972b-40fab7c6dd31")]
        [AssociationId("54c659d3-98ff-45e6-b734-bc45f13428d8")]
        [RoleId("d75a5613-4ed9-494f-accf-352d9e115ba9")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        C2[] I1C2One2Manies { get; set; }


        #region Allors
        [Id("593914b1-af95-4992-9703-2b60f4ea0926")]
        [AssociationId("ee0f3844-928b-4968-9077-afd255554c8b")]
        [RoleId("bca02f1e-a026-4c0b-9762-1bd52d49b953")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        C1 I1C1One2One { get; set; }


        #region Allors
        [Id("5cb44331-fd8c-4f73-8994-161f702849b6")]
        [AssociationId("2484aae6-db3b-4795-be76-016b33cbb679")]
        [RoleId("c9f9dd15-54b4-4847-8b7e-ac88063804a3")]
        #endregion
        int I1AllorsInteger { get; set; }


        #region Allors
        [Id("6199e5b4-133d-4d0e-9941-207316164ec8")]
        [AssociationId("75342efb-659c-43a9-8340-1e110087141c")]
        [RoleId("920f26a7-971a-4771-81b1-af3972c997ff")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        C2[] I1C2Many2Manies { get; set; }


        #region Allors
        [Id("670c753e-8ea0-40b1-bfc9-7388074191d3")]
        [AssociationId("b1c6c329-09e3-4b07-8ddf-e6a4fd8d0285")]
        [RoleId("6d36c9f9-1426-46a5-8d4f-7275a51c9c17")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        I1[] I1I1One2Manies { get; set; }


        #region Allors
        [Id("6bb3ba6d-ffc7-4700-9723-c323b9b2d233")]
        [AssociationId("86623fe9-c7cc-4328-85d9-b0dfce2b9a59")]
        [RoleId("9c64a761-136a-43aa-bef9-6bcd1259d591")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        I1[] I1I1Many2Manies { get; set; }


        #region Allors
        [Id("6c3d04be-6f95-44b8-863a-245e150e3110")]
        [AssociationId("e6c314af-d366-4169-b28d-9dc83d694079")]
        [RoleId("631a2bdb-ceca-43b2-abb8-9c9ea743c9de")]
        #endregion
        bool I1AllorsBoolean { get; set; }


        #region Allors
        [Id("818b4013-5ef1-4455-9f0d-9a39fa3425bb")]
        [AssociationId("335902bc-6bfa-4c7b-b52f-9a617c746afd")]
        [RoleId("56e68d93-a62f-4090-a93a-8f0f364b08bd")]
        [Precision(10)]
        [Scale(2)]
        #endregion
        decimal I1AllorsDecimal { get; set; }


        #region Allors
        [Id("a51d9d21-40ec-44b9-853d-8c18f54d659d")]
        [AssociationId("1d785350-3f68-4f8d-86d4-74a0cd8adac7")]
        [RoleId("222d2644-197d-4420-a01a-276b35ad61d1")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        I12 I1I12One2One { get; set; }


        #region Allors
        [Id("a5761a0e-5c10-407a-bd68-0c4f69d78968")]
        [AssociationId("b6cf882a-e27a-40e3-9a0d-43ade4d236b6")]
        [RoleId("3950129b-6ac5-4eae-b5c2-de12500b0561")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        I2 I1I2One2One { get; set; }


        #region Allors
        [Id("b6e0fce0-14fc-46e3-995d-1b6e3699ed96")]
        [AssociationId("ddc18ebf-0b61-441f-854a-0f964859035e")]
        [RoleId("3899bad1-d563-4f65-85b1-2b274b6a278f")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        C2 I1C2One2One { get; set; }


        #region Allors
        [Id("b89092f1-8775-4b6a-99ef-f8626bc770bd")]
        [AssociationId("d0b99a68-2104-4c4d-ba4c-73d725e406e9")]
        [RoleId("6303d423-6cc4-4933-9546-4b6b39fa0ae4")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        C1[] I1C1One2Manies { get; set; }


        #region Allors
        [Id("b9c67658-4abc-41f3-9434-c8512a482179")]
        [AssociationId("ba4fa583-b169-4327-a60a-fc0d2c142b3f")]
        [RoleId("bbd469af-25f5-47aa-86f6-80d3bba53ce5")]
        [Size(-1)]
        #endregion
        byte[] I1AllorsBinary { get; set; }


        #region Allors
        [Id("bcc9eee6-fa07-4d37-be84-b691bfce24be")]
        [AssociationId("b6c7354a-4997-4764-826a-0c9989431d3b")]
        [RoleId("7da3b7ea-2e1a-400c-adbf-436d35720ae9")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        C1[] I1C1Many2Manies { get; set; }


        #region Allors
        [Id("cdb758bf-ecaf-4d99-88fb-58df9258c13c")]
        [AssociationId("62961c44-f0ab-4edf-9aa7-63312643e6b4")]
        [RoleId("e33e809e-bbd3-4ecc-b46e-e233c5c93ce6")]
        #endregion
        double I1AllorsDouble { get; set; }


        #region Allors
        [Id("e1b13216-7210-4c24-a668-83b40162a21b")]
        [AssociationId("f14f50da-635f-47d0-9f3d-28364b767637")]
        [RoleId("911abf5b-ea84-4ffe-b6fb-558b4af50503")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        I1 I1I1One2One { get; set; }


        #region Allors
        [Id("e3126228-342a-4415-a2e8-d52eceaeaf89")]
        [AssociationId("202575b6-aaff-46ce-9e8a-e976a8a9d263")]
        [RoleId("2598d7df-a764-4b6e-bf91-5234404b97c2")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        C1 I1C1Many2One { get; set; }


        #region Allors
        [Id("e386cca6-e738-4c37-8bfc-b23057d7a0be")]
        [AssociationId("a3af5653-20c0-410c-9a6f-160e10e2fe69")]
        [RoleId("6c708f4b-9fb1-412b-84c8-02f03efede5e")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        #endregion
        I12[] I1I12One2Manies { get; set; }


        #region Allors
        [Id("ef1a0a5e-1794-4478-9d0a-517182355206")]
        [AssociationId("7b80b14e-dd35-4e7c-ba85-ac7860a5dc28")]
        [RoleId("1d51d303-f68b-4dca-9299-a6376e13c90e")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        C2 I1C2Many2One { get; set; }


        #region Allors
        [Id("f9d7411e-7993-4e43-a7e2-726f1e44e29c")]
        [AssociationId("84ae4441-5f83-4196-8439-483311b05055")]
        [RoleId("5ebf419f-1c7f-46f2-844c-0f54321888ee")]
        #endregion
        Guid I1AllorsUnique { get; set; }
        
        [Id("A360CF09-7B55-421B-A45D-D100BAF3D0D6")]
        void InterfaceMethod();
    }
}