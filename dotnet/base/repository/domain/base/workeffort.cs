// <copyright file="WorkEffort.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

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
        [Id("15F7EFA9-5A93-4921-8C42-D9CEC1F0EA63")]
        [AssociationId("0CB7203D-3745-453B-AE00-E4ED11DB059D")]
        [RoleId("D57F7A30-5D2A-4B7E-9E66-31C1D4EEDF90")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Organisation TakenBy { get; set; }

        #region Allors
        [Id("3B6366EB-AD53-4F35-AE0F-EFD9503CF271")]
        [AssociationId("E74362AF-2411-46F0-810E-6E15465C82CC")]
        [RoleId("650A22EE-F840-40BA-A297-A393370D2EAF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Organisation ExecutedBy { get; set; }

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
        [Required]
        [Workspace]
        Party Customer { get; set; }

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
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("B8348A76-34BF-4B1B-B840-0946C52A639D")]
        [AssociationId("7177F83A-B722-4B9B-8473-C100A164649A")]
        [RoleId("C08F12DD-92FE-4C01-9BD9-EAA484E42F87")]
        #endregion
        [Size(-1)]
        [Workspace]
        string WorkDone { get; set; }

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
        [Id("1cac44f2-bf48-4b7b-9d29-658e6eedeb86")]
        [AssociationId("ade49717-2f6b-4574-a6af-03d552ced0b2")]
        [RoleId("495fbe3c-8433-4593-bf32-ccfcc11b2a45")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        DateTime ActualStart { get; set; }

        #region Allors
        [Id("30645381-bb0c-4782-a9cc-388c7406456d")]
        [AssociationId("1187cb7d-4346-4089-b02c-b834a3ff8dca")]
        [RoleId("754b32ab-f426-41a9-87c1-b701f7952d15")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        DateTime ActualCompletion { get; set; }

        #region Allors
        [Id("aedad096-b297-47b7-98e4-69c6dde9b128")]
        [AssociationId("13208331-e72f-4adb-9e78-c6ba7b68ce65")]
        [RoleId("e23b0aa8-8b02-4274-826c-af140683ad22")]
        #endregion
        [Indexed]
        [Required]
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
        [Id("f5580447-3ac9-4bef-82db-f3e98652fae7")]
        [AssociationId("4498abb8-d330-4332-8ad6-0f82e7469666")]
        [RoleId("eb49972a-e939-476d-bc1d-be3eb7a9c7ef")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalLabourCost { get; set; }

        #region Allors
        [Id("55ed26a9-c025-4232-87d5-99d8f5cfd108")]
        [AssociationId("af876fc4-b63a-4ce8-8755-11233d955020")]
        [RoleId("334cc51a-9b1c-43f8-b569-9736c3ecee51")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalMaterialCost{ get; set; }

        #region Allors
        [Id("5fdb39db-0777-447b-9eb2-239a5aec0383")]
        [AssociationId("944efec8-111a-496c-a4e1-e13b99a2af62")]
        [RoleId("c86eaa44-032b-4202-a3fa-b97dcabbf005")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalSubContractedCost { get; set; }

        #region Allors
        [Id("1cf26872-3f96-430c-a4b8-cfde33cf628c")]
        [AssociationId("2819b06a-19b1-4050-a4e3-7af269478580")]
        [RoleId("5d019410-f620-435c-b41e-5e73b2e62898")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalCost { get; set; }

        #region Allors
        [Id("534a72d6-4b54-466c-a041-733b2a28594f")]
        [AssociationId("dcf12113-a7a8-448a-9b41-89ad74af7185")]
        [RoleId("786ff763-1a6a-4587-a570-e341a1ea0818")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalLabourRevenue { get; set; }

        #region Allors
        [Id("7aa98d1d-41bb-4ae7-9b6f-9cedaff4248a")]
        [AssociationId("898b7609-30f9-428c-91ff-791eea960380")]
        [RoleId("aea08711-4027-4174-84ec-cfe410da61cf")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalMaterialRevenue { get; set; }

        #region Allors
        [Id("7768e1d0-4b7b-41c3-a403-83ddf7117d16")]
        [AssociationId("a2b39171-72b5-4cf9-a924-2ad5774f21ce")]
        [RoleId("8502bdbf-6f18-44ce-92f2-8ba0bbb7c712")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalSubContractedRevenue { get; set; }

        #region Allors
        [Id("1c16f9d0-9d2e-47cc-b097-e63d4de43ea6")]
        [AssociationId("70eed9e3-b3a4-4923-aa4d-cab2eff2daf1")]
        [RoleId("b0a56062-3606-459c-a8d5-e8946ae90103")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalRevenue { get; set; }

        #region Allors
        [Id("e9d47579-1eb2-4023-953d-64b6826e82ac")]
        [AssociationId("5bacd50e-9471-4fd4-8d49-e70b727df833")]
        [RoleId("a8cdccb5-c8aa-426d-8e3f-1630606f701b")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal GrandTotal { get; set; }

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
        [Id("2efd427f-daeb-4b84-9f86-857ed1bdb1b7")]
        [AssociationId("0e92f113-f607-46bb-85c1-eb3cddb317ef")]
        [RoleId("40e23b5c-8943-4e27-86a1-d0a0140068e6")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
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
        [Id("76EEC72E-3636-42FD-9F50-B1A2818A2CC3")]
        [AssociationId("3A04E2E9-8B9E-42B5-8CA4-27B644A1CF23")]
        [RoleId("71991371-B74B-4DA0-B95D-8F54F4AFF447")]
        #endregion
        [Derived]
        [Required]
        [Workspace]
        bool CanInvoice { get; set; }

        #region Allors
        [Id("f013bc49-c622-48ed-a6d0-1cf677405498")]
        [AssociationId("964fb348-85e7-4c5d-a213-219a0c628581")]
        [RoleId("f235c729-f76c-494d-a301-d3225d04ca56")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public Media[] ElectronicDocuments { get; set; }

        #region Allors
        [Id("d21f9909-1dfd-4c7d-9559-68fb1fc29f26")]
        [AssociationId("fe2dda86-029a-46a8-83cd-f3c92525a3c1")]
        [RoleId("2151c494-2f3e-45b4-a598-b3d025be2205")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        int SortableWorkEffortNumber { get; set; }

        #region Allors
        [Id("163a5552-9436-4ecf-9b44-9f3cc6ab488f")]
        [AssociationId("60c3a0d0-2904-47d9-a3f3-8c02edde0e0f")]
        [RoleId("f25b9758-0988-4349-8671-24c04e814267")]
        #endregion
        [Required]
        [Workspace]
        Guid DerivationTrigger { get; set; }

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

        #region Allors
        [Id("506B288D-94C1-4157-A244-B62887BEA609")]
        #endregion
        [Workspace]
        void Invoice();

        #region Allors
        [Id("ea3b83de-aeac-406d-aaf3-0fcfd57bcaef")]
        #endregion
        [Workspace]
        void Revise();

        #region Allors
        [Id("6040f364-eec8-4672-add6-4d2590028d50")]
        #endregion
        [Workspace]
        void CalculateTotalRevenue();
    }
}
