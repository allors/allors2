namespace Allors.Repository
{
    using System;
    using Attributes;
    
    #region Allors
    [Id("8407B4DF-88BF-43E5-89DA-999A32B16CF5")]
    #endregion
    public partial interface CommunicationEventVersion : Version
    {
        #region Allors
        [Id("2B30E8D7-CF09-448C-9339-D90C5111CF6E")]
        [AssociationId("99FE354D-24CB-4594-B450-69352438625C")]
        [RoleId("8852E08C-1F0B-41C6-9557-6E33A0827F90")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        CommunicationEventState CommunicationEventState { get; set; }

        #region Allors
        [Id("A4FD08D4-8665-4740-A3D0-04A7DF1B019E")]
        [AssociationId("493FA7C5-7D13-4407-8676-1A24A54451A9")]
        [RoleId("1C5D1C7D-A1F2-420E-9D64-BBAC4105CC0C")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Comment { get; set; }

        #region Allors
        [Id("660C7D51-2EF0-416A-8136-767469DBD9A1")]
        [AssociationId("C9834736-1F0F-467D-A0FA-CA0AA8D31350")]
        [RoleId("FC832A87-719A-48C1-B4AA-82E7508F8B59")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User CreatedBy { get; set; }

        #region Allors
        [Id("7E772D4D-85E7-4BD9-83C2-2FBA978CB867")]
        [AssociationId("C91F1B00-976C-4EAA-AF61-6DBD2B112ABB")]
        [RoleId("359F2FAD-17FC-420B-8D4D-18C88CFD8C2D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User LastModifiedBy { get; set; }

        #region Allors
        [Id("763B74F2-5CBA-4868-A405-CAE9283175AE")]
        [AssociationId("3A73A933-5A4F-4801-8BCC-7A1A8A70C240")]
        [RoleId("766AD051-4F5E-484A-8FB0-D5AA79168965")]
        #endregion
        [Workspace]
        DateTime CreationDate { get; set; }

        #region Allors
        [Id("07F06A16-BB1E-48D8-919B-4DFF31A1F4FD")]
        [AssociationId("A95AD68F-E9CC-4FAD-92EB-157D875376DC")]
        [RoleId("8342CEB8-03BF-4557-8294-E64BA205F92B")]
        #endregion
        [Workspace]
        DateTime LastModifiedDate { get; set; }

        #region Allors
        [Id("9BB7DC70-AA96-4936-B20E-E3CB56E15126")]
        [AssociationId("0FF04FF1-51A6-44A2-867F-C423C0769AE9")]
        [RoleId("5015C868-E1B6-47F3-886D-B6DF3A609310")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Derived]
        SecurityToken OwnerSecurityToken { get; set; }

        #region Allors
        [Id("B787B7AF-78E0-4837-A28C-1B3DB30308B6")]
        [AssociationId("F66A122B-FDB8-4545-AF69-83046150FDC6")]
        [RoleId("3D5644BF-91D7-4051-BFA3-32C9BE8D7FCE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Derived]
        AccessControl OwnerAccessControl { get; set; }

        #region Allors
        [Id("9D549ABA-3EB6-44C1-BB29-BEF13A50D41E")]
        [AssociationId("339906B2-B895-4C7B-9C09-6FD821E0B904")]
        [RoleId("06766417-AC8A-4E8A-AC40-1C56C8F3B57B")]
        #endregion
        [Workspace]
        DateTime ScheduledStart { get; set; }

        #region Allors
        [Id("2759C5DA-45E4-4715-83C7-B68A51E213DB")]
        [AssociationId("45A51D1E-43A7-466A-A234-D8718D0BE719")]
        [RoleId("EA779517-7E3A-4C72-A635-BAFBAA422594")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        Party FromParty { get; set; }

        #region Allors
        [Id("81B2EC68-0FF0-49D9-813B-E32BBC3F8872")]
        [AssociationId("56D1C928-6FFD-4A0A-ACED-6A7A761C1C1E")]
        [RoleId("2DC14B24-C307-44AD-A2CE-20922FA76ECE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        Party ToParty { get; set; }

        #region Allors
        [Id("0273F21F-8794-47FD-A5D9-34912D54C281")]
        [AssociationId("5B2B41D4-DBEA-429A-B289-E25C9E45644B")]
        [RoleId("863905C8-5253-4B9C-9F37-A2DD572B4117")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        ContactMechanism[] ContactMechanisms { get; set; }

        #region Allors
        [Id("E523242D-553F-4A2D-8663-072CCC693089")]
        [AssociationId("F46F8501-620A-4EA5-B5D5-1264753C8273")]
        [RoleId("7709DE09-98B1-4859-A944-B1DBC89CDFE7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] InvolvedParties { get; set; }

        #region Allors
        [Id("5E653B50-7BC1-4D0A-BA2F-965A8FC4AD6C")]
        [AssociationId("F518ABBE-3339-4F18-B1D4-E9FB7D01E2E1")]
        [RoleId("09CCF0D9-5BC6-4D27-B05B-7BB68EACEB0A")]
        #endregion
        [Workspace]
        DateTime InitialScheduledStart { get; set; }

        #region Allors
        [Id("8FFCBA83-FEE3-42C8-9102-F2F0380A411A")]
        [AssociationId("A13F5C38-60A5-40ED-9459-D4E2B77FC7A1")]
        [RoleId("CD0E0C28-82D6-4A15-AFCE-DA0D04D4EF24")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        CommunicationEventPurpose[] EventPurposes { get; set; }

        #region Allors
        [Id("D0E28A3B-0136-453B-A4A5-3117C47F7F1E")]
        [AssociationId("F6A57963-D490-43FD-8D8F-9CB8A32BB087")]
        [RoleId("3E1A0154-5B55-4C0C-8CAB-80B39EAC8BF0")]
        #endregion
        [Workspace]
        DateTime ScheduledEnd { get; set; }

        #region Allors
        [Id("37C90ED0-40BA-4210-88E2-8539EAB440A9")]
        [AssociationId("B8005243-8E85-4A90-8A8E-D65B582D9530")]
        [RoleId("9DD4076B-6A8E-4222-AF9E-59C2E63FF7B4")]
        #endregion
        [Workspace]
        DateTime ActualEnd { get; set; }

        #region Allors
        [Id("8CC7CCAD-293D-4264-A20F-558159673273")]
        [AssociationId("B19A3BE7-78B8-4EF9-B143-D3AB8DF9321B")]
        [RoleId("C80CCB7A-87FA-4850-B291-0C1F44106096")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] WorkEfforts { get; set; }

        #region Allors
        [Id("FB90D1C1-0A0D-4C31-BF4C-4D0CA255231C")]
        [AssociationId("52318CCA-9DAF-4F85-A78D-3B5A6698A2D1")]
        [RoleId("69A6B60B-506E-42B8-B19E-3216CD81C7CE")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("B5188C59-241A-4695-9C3D-A162D44A7240")]
        [AssociationId("6C003630-E45C-4368-8D65-FE4AA2ACB833")]
        [RoleId("3BB8ADE8-858F-4B37-B0A8-34C2AF36FBDF")]
        #endregion
        [Workspace]
        DateTime InitialScheduledEnd { get; set; }

        #region Allors
        [Id("22D2407E-86FE-46FB-9C59-E92306330027")]
        [AssociationId("187D3545-3E45-421C-8219-6E2E715F7423")]
        [RoleId("7A7328BC-F9D7-492E-B84B-B14C3A8A0290")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        string Subject { get; set; }

        #region Allors
        [Id("9AD00AF2-8DFA-4167-9619-BD9C265DADFC")]
        [AssociationId("F1D562E4-E7C6-4A4C-909E-6402A29B0076")]
        [RoleId("1FDF2A88-2BCB-4CF7-8D11-11FF3FE5DB9C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Media[] Documents { get; set; }

        #region Allors
        [Id("325F7DE6-356D-4A21-913A-4EFE6EDE6A95")]
        [AssociationId("35A3BFD3-1425-4826-870A-1791BDCF510C")]
        [RoleId("E6F95710-2189-4707-A81A-6933956E7B9E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Case Case { get; set; }

        #region Allors
        [Id("26063624-EDAF-4E2F-A2FC-75523471FD3F")]
        [AssociationId("1D695A81-40FE-4B8A-A6C0-D2B12263C6EA")]
        [RoleId("4488D172-096B-48A7-9C6B-4001D2966F21")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Priority Priority { get; set; }

        #region Allors
        [Id("26868123-D390-4EB2-8570-EB26BE158D1B")]
        [AssociationId("6618C93E-E1A3-4BA3-A453-D184E6C9B4EF")]
        [RoleId("C282F370-1F9B-40CE-8CCD-F84744312B5D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Person Owner { get; set; }

        #region Allors
        [Id("4081E134-3D15-474F-9C0E-8E7C20CE5EB2")]
        [AssociationId("0045D94E-B1E9-4D32-8A05-275FA2C7E93A")]
        [RoleId("92E974BC-221D-4334-BCD5-67D18A96AD19")]
        #endregion
        [Workspace]
        DateTime ActualStart { get; set; }

        #region Allors
        [Id("B5DF3131-04F2-4DEF-BC41-2581F3CC5923")]
        [AssociationId("3CF72EAC-601D-43A6-91BA-40CE0DA8DE6D")]
        [RoleId("C008BD54-4838-4BE9-BAE7-A72A99D0D121")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        bool SendNotification { get; set; }

        #region Allors
        [Id("C7CE474E-1677-4EF2-ABF9-CE5FF26CA075")]
        [AssociationId("1A3D438C-A259-4DC2-962A-5B0BAD9B8C99")]
        [RoleId("EBEFA638-86D8-45DB-8316-6DF8AEA8A043")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        bool SendReminder { get; set; }

        #region Allors
        [Id("48329980-DACF-436E-A8B2-2E46CE0B07C3")]
        [AssociationId("11CBCD2A-0090-4AB1-BDBC-6DC915F7F48C")]
        [RoleId("5A65E336-02A9-4C2C-90FE-3191F3EEB91F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        DateTime RemindAt { get; set; }
    }
}