namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("c5747a64-f468-4d0d-80f3-6463bd32b0ca")]
    #endregion
    public partial interface S12 : Object
    {


        #region Allors
        [Id("06fabe71-737a-4cff-ac10-2d15dafce503")]
        [AssociationId("f3b1ecf3-95d6-4b96-893e-4ffa0c69bc72")]
        [RoleId("cb10aafe-0330-482c-8782-4c50fc56b00e")]
        [Size(256)]
        #endregion
        string S12AllorsString { get; set; }


        #region Allors
        [Id("2eb9e232-4ed4-4997-a21a-f11bb0fe3b0e")]
        [AssociationId("7d0f87c2-8309-4bb2-afac-e6f311127f8e")]
        [RoleId("d4003255-3083-428a-b2af-9456313cd765")]
        #endregion
        DateTime S12AllorsDateTime { get; set; }


        #region Allors
        [Id("39f50108-df59-455d-8371-fc07f3dbb7ef")]
        [AssociationId("458190b2-3823-4b79-a17f-94a30daf1c35")]
        [RoleId("c2cc8dd6-7154-47cd-a182-bd0034701c4f")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        C2[] S12C2many2manies { get; set; }


        #region Allors
        [Id("61e8c425-407e-408b-9f2e-c95548833004")]
        [AssociationId("e48a6ba4-1b90-43e4-ae17-2cef1209cc2c")]
        [RoleId("dcab1da2-1e9c-4019-bc72-16af5e6e791b")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        C2 S12C2many2one { get; set; }


        #region Allors
        [Id("830117d4-fbe1-4944-bacf-54331e8451d7")]
        [AssociationId("f66241ad-4692-4907-92cf-f7a49aa6fe70")]
        [RoleId("179225dd-73a2-4695-b21f-0d4070d90bdf")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        C2 S12C2one2one { get; set; }


        #region Allors
        [Id("a3aac482-aad0-4b59-9361-51b23867e5a2")]
        [AssociationId("f63d8a2d-257d-459b-98be-73847a54a91d")]
        [RoleId("e3f037d9-70f7-4ab2-a14f-645efd1528b9")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        C2[] S12C2one2manies { get; set; }


        #region Allors
        [Id("a97eca8e-807b-4a06-9587-6240f6150203")]
        [AssociationId("d2c367ed-8001-4f16-882c-64d3e30da8d1")]
        [RoleId("e86db1ff-d072-42eb-9493-bc01984b7d8d")]
        #endregion
        bool S12AllorsBoolean { get; set; }


        #region Allors
        [Id("acc4ae39-2d5c-4485-be22-87b27e84b627")]
        [AssociationId("2b86671d-b870-4007-b564-bb0c10b40bc3")]
        [RoleId("64d27877-edf1-4136-a810-19a5d5356110")]
        #endregion
        double S12AllorsDouble { get; set; }


        #region Allors
        [Id("d07313ca-fd8d-4c74-928e-41274aa28de9")]
        [AssociationId("d48b29d7-3b17-4cb5-b8f8-e85bc86876cd")]
        [RoleId("a4163284-3b87-46c0-9495-acf5e3240513")]
        #endregion
        int S12AllorsInteger { get; set; }


        #region Allors
        [Id("f7ace363-89bd-4ea5-a865-4a6e3de2d723")]
        [AssociationId("c67514c2-5b6f-4eb5-b730-eb74642ff6e7")]
        [RoleId("68937f6c-9561-49e8-abb3-24e0eeabfcb1")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal S12AllorsDecimal { get; set; }

    }
}