namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("553a5280-a768-4ba1-8b5d-304d7c4bb7f1")]
    #endregion
    public partial interface WorkEffort : Transitional, UniquelyIdentifiable, Deletable, Auditable, Commentable, Printable
    {
        #region WorkEffortState
        #region Allors
        [Id("4240C679-19EA-41B9-A82D-D156DE6B4007")]
        [AssociationId("131CCEE8-3062-4B9F-B671-03C1425DC97F")]
        [RoleId("FBF69E37-1A71-406E-BF4D-F36BF1145FAD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        WorkEffortState PreviousWorkEffortState { get; set; }

        #region Allors
        [Id("0144D0DC-B981-4FD2-B6D1-1629F6957A5D")]
        [AssociationId("5F41D7B1-D2F8-4131-BA01-9EA7DA5556B4")]
        [RoleId("ED98EC81-14AF-496E-B0CB-8DE2082FE985")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        WorkEffortState LastWorkEffortState { get; set; }

        #region Allors
        [Id("22325B2E-AD74-4B12-9F5B-856858B002DD")]
        [AssociationId("530B6F04-6AD3-43D5-9E5A-BA28F58BA1D1")]
        [RoleId("D4713F5C-60D2-4285-99DC-6F3B6654EAA9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        WorkEffortState WorkEffortState { get; set; }
        #endregion

        #region Allors
        [Id("7CDA479C-E7D8-4EE2-8005-7CF73CC88819")]
        [AssociationId("E7633664-4A2C-4D68-9D26-9096FBD41171")]
        [RoleId("2637A073-0E41-4B0B-BDD0-F2F48BE588BC")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        [Derived]
        SecurityToken OwnerSecurityToken { get; set; }

        #region Allors
        [Id("0315B5F6-F35C-4C7D-9CC9-F9E2DA4C19DB")]
        [AssociationId("0AEFF8C3-F18B-4B50-A865-5FEB0549A7C6")]
        [RoleId("1734195B-F174-4832-8DF8-B5D7AD05E497")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Required]
        [Derived]
        AccessControl OwnerAccessControl { get; set; }

        #region Allors
        [Id("15F7EFA9-5A93-4921-8C42-D9CEC1F0EA63")]
        [AssociationId("0CB7203D-3745-453B-AE00-E4ED11DB059D")]
        [RoleId("D57F7A30-5D2A-4B7E-9E66-31C1D4EEDF90")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Organisation TakenBy { get; set; }

        //#region Allors
        //[Id("C814C85A-2C07-4C65-9AEA-C889425C8EE3")]
        //[AssociationId("5586602B-68F0-4409-AAFA-32BA3C20AE07")]
        //[RoleId("5D076AD3-0E19-48CA-A6A2-E825BBE0F3BA")]
        //#endregion
        //[Multiplicity(Multiplicity.ManyToOne)]
        //[Indexed]
        //[Required]
        //[Workspace]
        //Store Store { get; set; }

        #region Allors
        [Id("0db9b217-c54f-4a7b-a1c0-9592eeabd51f")]
        [AssociationId("c918d8f5-77f0-4c0d-b02a-7695a7109cf2")]
        [RoleId("ae8f325d-31e5-473a-8caf-d378ba571025")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Facility Facility { get; set; }

        #region Allors
        [Id("5012F30D-1B22-47D7-B3A5-42F023FEE3E1")]
        [AssociationId("C8AD98B1-551E-40DA-AE5E-2E2B4AED9748")]
        [RoleId("F9171EC9-1CEF-431F-8640-763DFF84E0F9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Party Customer{ get; set; }

        #region Allors
        [Id("2C866A1C-AC26-468A-B01F-5A0D8FFF7513")]
        [AssociationId("DD885CBA-187B-420E-8CD9-21F8A5BDED54")]
        [RoleId("EFBA5010-4C8F-43EF-92ED-68348E5E62D5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        ContactMechanism FullfillContactMechanism { get; set; }

        #region Allors
        [Id("DE8ABB5B-E0CB-4FDA-AF49-D6359E909E31")]
        [AssociationId("98FA64C0-2CBA-4B4D-8B3F-EBAB68314F2F")]
        [RoleId("EC156A38-494C-47A4-B199-23ED1F005A2A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Person ContactPerson { get; set; }

        #region Allors
        [Id("E938CD9B-C1E3-4DA6-BB0A-1DF917061A56")]
        [AssociationId("D8E85B42-A9BC-4915-97D9-C34CAA81B8B3")]
        [RoleId("8C0BF8BD-DF48-4811-8264-CC13CFE2E299")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Size(256)]
        string WorkEffortNumber { get; set; }

        #region Allors
        [Id("3C1D7BA5-A031-4890-85C8-0119EF754F5D")]
        [AssociationId("C582BA86-6D49-4EB5-B1A1-A3A69FA3E07F")]
        [RoleId("23694E48-CB38-4AB5-8FE2-47FF7B206306")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Person Owner { get; set; }

        #region Allors
        [Id("97a874e9-10ef-43fb-80d2-10e0974bb3a1")]
        [AssociationId("29df5d80-5baf-436c-b4ae-48f2f0dad2fd")]
        [RoleId("bf8f5058-dd2c-439d-bf7c-879ab69a2ca1")]
        #endregion
        [Size(256)]
        [Required]
        [Workspace]
        string Name { get; set; }

        #region Allors
        [Id("bac1939b-8cf8-4b18-862c-4c2dc0a591e5")]
        [AssociationId("7172728e-29d2-498f-bea9-da8ab04a1ae5")]
        [RoleId("60306059-f537-4fd6-9d31-7b502f39662e")]
        #endregion
        [Size(4096)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("858e42df-d775-4eec-b029-343e8b094311")]
        [AssociationId("7abda175-3c95-46c7-b7a9-35aafca3df1c")]
        [RoleId("0490c978-ec91-4418-a5cc-bb014b428dcf")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Priority Priority { get; set; }

        #region Allors
        [Id("6af30dd9-7f3b-47e7-a929-7ecd28b9b53f")]
        [AssociationId("74684daa-d716-4af7-b819-0ab224077eac")]
        [RoleId("e4485ae7-5156-4b97-aaa2-fe7fe6f80699")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        WorkEffortPurpose[] WorkEffortPurposes { get; set; }

        #region Allors
        [Id("30645381-bb0c-4782-a9cc-388c7406456d")]
        [AssociationId("1187cb7d-4346-4089-b02c-b834a3ff8dca")]
        [RoleId("754b32ab-f426-41a9-87c1-b701f7952d15")]
        #endregion
        [Indexed]
        [Workspace]
        DateTime ActualCompletion { get; set; }

        #region Allors
        [Id("aedad096-b297-47b7-98e4-69c6dde9b128")]
        [AssociationId("13208331-e72f-4adb-9e78-c6ba7b68ce65")]
        [RoleId("e23b0aa8-8b02-4274-826c-af140683ad22")]
        #endregion
        [Indexed]
        [Workspace]
        DateTime ScheduledStart { get; set; }

        #region Allors
        [Id("e189f9fc-fe3c-44af-985a-cdc3e749e25c")]
        [AssociationId("9747c7a3-7f5c-4660-9cb3-3635acd954a0")]
        [RoleId("25dcbbaa-c53a-44de-b248-46b9ec5dfed2")]
        #endregion
        [Indexed]
        [Workspace]
        DateTime ScheduledCompletion { get; set; }

        #region Allors
        [Id("b6213705-ed58-4597-9939-a058b89610f8")]
        [AssociationId("4ad69693-3a44-4403-abed-43fd6f208348")]
        [RoleId("21381f45-898c-4622-9e26-039cb49a9eaa")]
        #endregion
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal ActualHours { get; set; }

        #region Allors
        [Id("ebd0daa8-ab45-4390-89f7-3bc89faecdfb")]
        [AssociationId("db761f6b-63e2-41fc-a5d9-1d80daa12fbe")]
        [RoleId("149d4820-8630-42a0-9458-18671fb09071")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal EstimatedHours { get; set; }

        #region Allors
        [Id("092a296d-6f15-4fdd-aed6-25185e6e10b1")]
        [AssociationId("95a67913-5914-4705-b76d-6eed73704fab")]
        [RoleId("ff1fade9-aa0b-4058-b8e0-8d993eb841cb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] Precendencies { get; set; }

        #region Allors
        [Id("1a3705c0-0e77-4d6d-a368-ef5141a6c908")]
        [AssociationId("b22db3e0-68aa-477c-b86b-96a1b2bb8d20")]
        [RoleId("3f80745d-6a22-4322-b349-ca2a7e441692")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Deliverable[] DeliverablesProduced { get; set; }

        #region Allors
        [Id("1cac44f2-bf48-4b7b-9d29-658e6eedeb86")]
        [AssociationId("ade49717-2f6b-4574-a6af-03d552ced0b2")]
        [RoleId("495fbe3c-8433-4593-bf32-ccfcc11b2a45")]
        #endregion
        [Indexed]
        [Workspace]
        DateTime ActualStart { get; set; }

        #region Allors
        [Id("2efd427f-daeb-4b84-9f86-857ed1bdb1b7")]
        [AssociationId("0e92f113-f607-46bb-85c1-eb3cddb317ef")]
        [RoleId("40e23b5c-8943-4e27-86a1-d0a0140068e6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] Children { get; set; }

        #region Allors
        [Id("3081fa56-272c-43d6-a54c-ad70cb233034")]
        [AssociationId("171d3338-5b58-4776-87de-a0b934e15a0a")]
        [RoleId("3c24f9fa-1ada-42f8-8fe1-90c244189254")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        OrderItem OrderItemFulfillment { get; set; }

        #region Allors
        [Id("3bebd379-65a9-445e-898e-8913c26b94e6")]
        [AssociationId("d12425ed-2676-419e-bfae-674810fde5a8")]
        [RoleId("f4b0fb7e-8e84-43ca-88b0-44242216ee7e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        WorkEffortType WorkEffortType { get; set; }

        #region Allors
        [Id("a60c8797-320d-471f-9755-d3ef20a4feac")]
        [AssociationId("dd8b0f11-0443-4120-be2f-9a43125ccd62")]
        [RoleId("7693cd03-9b2c-4f10-9826-0335371e893d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Requirement[] RequirementFulfillments { get; set; }

        #region Allors
        [Id("a6fa6291-501a-4b5e-992d-ee5b9a291700")]
        [AssociationId("c5cbd6e4-8a61-4e7b-9219-55170ef79f3e")]
        [RoleId("8e8c2f0e-562f-4cb8-9b3f-6a255df820a3")]
        #endregion
        [Size(-1)]
        [Workspace]
        string SpecialTerms { get; set; }

        #region Allors
        [Id("add2f3d5-d83a-4734-ad69-9f86eb116f06")]
        [AssociationId("d5f050e0-d662-4ac7-90d5-16625fd4afff")]
        [RoleId("18fac5c8-2ba6-43cb-ad3b-d82facc17590")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] Concurrencies { get; set; }

        #region Allors
        [Id("B95571A0-84DF-4648-80FD-C4FE9067991F")]
        #endregion
        [Workspace]
        void Confirm();

        #region Allors
        [Id("46D78F1B-D77A-4240-87AB-14934BA12761")]
        #endregion
        [Workspace]
        void Finish();

        #region Allors
        [Id("D9234724-215F-4F6C-B3E8-9743CB22A245")]
        #endregion
        [Workspace]
        void Cancel();

        #region Allors
        [Id("F581B87C-EE9D-4D43-9719-8BC5CCFAC2C3")]
        #endregion
        [Workspace]
        void Reopen();

        #region Allors
        [Id("A0DA49D5-9AB3-4F1C-A40C-81644ADDD411")]
        #endregion
        [Workspace]
        void Complete();
    }
}