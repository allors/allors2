// <copyright file="WorkEffortVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region
    [Id("E86A7C06-1376-42C0-B901-2F64C6D0B1A6")]
    #endregion
    public partial interface WorkEffortVersion : Version
    {
        #region Allors
        [Id("D2282505-6967-412B-8D92-53D10A8BE7BE")]
        [AssociationId("C6EA0D53-C552-4B8F-9F90-B8FB838C4F39")]
        [RoleId("D6626950-A396-41D5-9FA4-EF906DD8DD8D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        WorkEffortState WorkEffortState { get; set; }

        #region Allors
        [Id("C8F73224-717F-4E55-99BC-23507CDE4967")]
        [AssociationId("202E3160-F290-4DEA-97BB-B3FB01A93B49")]
        [RoleId("0AF7FAC8-043E-4A41-B7CF-2137970243FF")]
        #endregion
        [Size(256)]
        [Required]
        [Workspace]
        string Name { get; set; }

        #region Allors
        [Id("33ECA579-D79A-488F-A9E5-B760C6DD2E29")]
        [AssociationId("46B9BF6C-A4E8-483F-A66E-E03EA6D5D2AF")]
        [RoleId("FB14FF04-78F5-4D8E-8260-20F3F9D7ADDE")]
        #endregion
        [Size(4096)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("4C4240A5-528B-497F-8846-AA7C99942C82")]
        [AssociationId("442BF907-56EA-4730-AA5B-912812132216")]
        [RoleId("08030219-963D-4F71-B6CB-3BE46B9EAFEE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Priority Priority { get; set; }

        #region Allors
        [Id("35A7A9A0-00C2-4548-BA8F-DCBDFDFD577E")]
        [AssociationId("BDCA5223-0476-460C-8311-78925579ECDB")]
        [RoleId("6EFAE079-6A84-48BC-AC7B-35AF31103C5F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        WorkEffortPurpose[] WorkEffortPurposes { get; set; }

        #region Allors
        [Id("1744851E-CD98-4F34-AFD1-B1096E4DC23E")]
        [AssociationId("DA8F50BD-26A6-49F2-9DDC-8677C7356931")]
        [RoleId("A9ABB966-64A3-42EB-85EC-623592DF00AE")]
        #endregion
        [Indexed]
        [Workspace]
        DateTime ActualCompletion { get; set; }

        #region Allors
        [Id("9415C622-BE7B-4927-9E72-D14D663BDDE6")]
        [AssociationId("0FFAF77A-274B-453D-90CB-00AEFC8B0C2E")]
        [RoleId("C0555048-8DBE-4B72-B03B-44E15A400D8A")]
        #endregion
        [Indexed]
        [Workspace]
        DateTime ScheduledStart { get; set; }

        #region Allors
        [Id("379F481B-D393-4FCE-9754-C5743A938524")]
        [AssociationId("0FFE8AAD-A198-428E-ABE5-49BDAD15AA07")]
        [RoleId("F928FFD5-4699-4D1C-B9BA-9C888AE21F97")]
        #endregion
        [Indexed]
        [Workspace]
        DateTime ScheduledCompletion { get; set; }

        #region Allors
        [Id("55CD6EE3-11CE-4546-8296-43E6C6AF0402")]
        [AssociationId("6D96D9E5-DD0B-4977-91E6-2DBA80AB4799")]
        [RoleId("B3812DC6-B555-4C8F-A3D9-F343D1EC21A8")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal ActualHours { get; set; }

        #region Allors
        [Id("97460292-E59B-4D71-90C7-31AC1FE5ADE4")]
        [AssociationId("F9ABBB6C-7A7B-4AF1-AE53-4869ABBD7211")]
        [RoleId("B042A75C-5AED-4263-8118-A7E1754B4CA9")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal EstimatedHours { get; set; }

        #region Allors
        [Id("1D7C6E6B-C871-4BC9-A0DE-77611BAC9F4A")]
        [AssociationId("6D1A680B-DEC7-4421-A76E-AD967D0AB11A")]
        [RoleId("7D4D8BA1-4D76-4387-9D8F-FD18FC01FF25")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] Precendencies { get; set; }

        #region Allors
        [Id("0954D5B6-228B-46F7-9929-99AEAC303F0D")]
        [AssociationId("5F762679-2F7D-4367-9DCE-F110CB6174EF")]
        [RoleId("A3D27645-82F4-4182-A671-B62B547A5EC1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Facility Facility { get; set; }

        #region Allors
        [Id("AF35EA36-E0F3-45F9-A865-1820576DBDEB")]
        [AssociationId("A6B3C602-0B53-422B-ABF0-9688DB395B61")]
        [RoleId("39073D58-0B07-4520-BFE1-0975D9410725")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Deliverable[] DeliverablesProduced { get; set; }

        #region Allors
        [Id("2185ECCD-1FA7-401D-9FF3-24601A587E62")]
        [AssociationId("0E1736BC-C00D-4071-922F-054A6A05CBD2")]
        [RoleId("1C44402C-5BA5-43AA-BC12-4CD6027CC128")]
        #endregion
        [Indexed]
        [Workspace]
        DateTime ActualStart { get; set; }

        #region Allors
        [Id("DC486C30-23A6-4996-B6F2-990D994B5678")]
        [AssociationId("64D65BA3-B4DC-4A33-B19F-13230A681C8F")]
        [RoleId("A2CAE0EB-B6DF-4550-B8D2-432224B036CD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] Children { get; set; }

        #region Allors
        [Id("5528643C-3081-4E1D-BA37-BB17BEC3FDEF")]
        [AssociationId("72AEE484-06EC-478F-BB63-32AFC67FA843")]
        [RoleId("9C29483F-4A70-4854-AFEB-9AAA198D7FFF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        OrderItem OrderItemFulfillment { get; set; }

        #region Allors
        [Id("96DEF640-D70D-47F0-AE44-FD4C084F8128")]
        [AssociationId("10C68045-AB79-465A-9200-F047479CAB58")]
        [RoleId("484E1F42-D7D2-41C5-B2A2-D90D1EDA42B7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        WorkEffortType WorkEffortType { get; set; }

        #region Allors
        [Id("AC3B999B-A4C9-4BFD-A666-6AD131DB6D37")]
        [AssociationId("B8DA50D6-8C52-4A7C-B8F4-7BEF0A8A563F")]
        [RoleId("295C5DA6-F592-47A3-B39D-284CE86ED0BA")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Requirement[] RequirementFulfillments { get; set; }

        #region Allors
        [Id("CF1F73FA-0517-4D5C-81B9-35EE1243309F")]
        [AssociationId("6A13A53E-319F-4EAF-9CAB-F1582BC8DEEE")]
        [RoleId("7E6D9D60-5E75-4F81-802F-758FC80AA94E")]
        #endregion
        [Size(-1)]
        [Workspace]
        string SpecialTerms { get; set; }

        #region Allors
        [Id("B0E17757-358A-4490-82A7-3CC54C8302B2")]
        [AssociationId("1ED8DE96-FFFA-4787-85C2-1CAE782977F6")]
        [RoleId("D097C09B-EA1D-410C-95B4-B521C625F7B6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        WorkEffort[] Concurrencies { get; set; }
    }
}
