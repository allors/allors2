namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("553a5280-a768-4ba1-8b5d-304d7c4bb7f1")]
    #endregion
    public partial interface WorkEffort : Transitional, UniquelyIdentifiable, Deletable, Auditable
    {
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
        [Id("d71aaad8-20ba-4e7f-a4f8-da43e372e202")]
        [AssociationId("a1a70f42-fba3-451c-8241-a854a4dba7e2")]
        [RoleId("e6d3f9cb-5465-44e2-92bf-0844c6dfe806")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        WorkEffortObjectState CurrentObjectState { get; set; }

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
        [Id("0db9b217-c54f-4a7b-a1c0-9592eeabd51f")]
        [AssociationId("c918d8f5-77f0-4c0d-b02a-7695a7109cf2")]
        [RoleId("ae8f325d-31e5-473a-8caf-d378ba571025")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Facility Facility { get; set; }

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
        [Id("2e7494ed-6df4-424e-907b-3b900aabf4c5")]
        [AssociationId("c6a502b8-5867-4ac9-8356-60155c1950ae")]
        [RoleId("45733b43-f02c-498d-8e77-fe882526268c")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        WorkEffortInventoryAssignment[] InventoryItemsNeeded { get; set; }

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
        [Id("62b84e6e-1b2f-46cb-825f-57f586e6cb92")]
        [AssociationId("81a938c3-1b27-4c24-993a-9bf616f06582")]
        [RoleId("dc2dc942-1210-4fdd-ad95-fe5b4dbd674d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        InventoryItem[] InventoryItemsProduced { get; set; }

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
    }
}