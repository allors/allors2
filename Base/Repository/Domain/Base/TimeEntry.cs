// <copyright file="TimeEntry.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("6360b45d-3556-41c6-b183-f42a15b9424f")]
    #endregion
    [Plural("TimeEntries")]
    public partial class TimeEntry : ServiceEntry, DelegatedAccessControlledObject
    {
        #region inherited properties
        public EngagementItem EngagementItem { get; set; }

        public bool IsBillable { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public string Description { get; set; }

        public WorkEffort WorkEffort { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("0BF79180-E5A6-44BD-ACC7-1A1563E29152")]
        [AssociationId("22C01EE9-AC26-4E49-8CE9-A84FC03B4FE3")]
        [RoleId("AA56EB9E-C4F0-4D04-875D-D79A36297603")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public Person Worker { get; set; }

        #region Allors
        [Id("E086CAE8-62C2-4892-AC97-004A811A3904")]
        [AssociationId("E671F25C-1B9F-465F-8D8E-41518233EFBA")]
        [RoleId("1C5168E6-1636-4D1F-BDAD-E6323F149FF9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public RateType RateType { get; set; }

        #region Allors
        [Id("1b07c419-42af-480b-87ba-1c001995dc51")]
        [AssociationId("2c605991-8d65-4b8f-9daf-e085af5b12c0")]
        [RoleId("90872970-372a-4f8d-9c53-c753aca9f99f")]
        #endregion
        [Required]
        [Derived]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Cost { get; set; }

        #region Allors
        [Id("1bb9affa-1390-4f54-92b5-64997e55525e")]
        [AssociationId("0f0341bb-d719-4989-a39b-02b1c1ce98b9")]
        [RoleId("ff8087ac-403d-46e4-b799-316bbdb6616e")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal GrossMargin { get; set; }

        #region Allors
        [Id("258a33cc-06ea-45a0-9b15-1b6d58385910")]
        [AssociationId("4909a04f-fd14-46ce-9c4c-bc7a2cc21914")]
        [RoleId("cff49ef3-5b51-4501-a5c8-59b4d5714f4e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public QuoteTerm QuoteTerm { get; set; }

        #region Allors
        [Id("E478A603-B2DA-4C76-91A5-96C5A737FCFC")]
        [AssociationId("518A24DB-10FC-42A5-A162-CBCBFFD09E1B")]
        [RoleId("15DA196E-9AE2-4CAE-B1BC-44FDD07FCF4D")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AssignedBillingRate { get; set; }

        #region Allors
        [Id("2c33de6e-b4fd-47e4-b254-2991f33f01f1")]
        [AssociationId("c8b7e4be-fbc5-414c-8e30-3947925c24b8")]
        [RoleId("1cca252a-d6a1-4945-991a-dd85090bb41d")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Derived]
        [Workspace]
        public decimal BillingRate { get; set; }

        #region Allors
        [Id("409ff1fb-1531-4829-9d6b-7b3e7113594a")]
        [AssociationId("54a57392-59ed-4583-99f1-1f2a97ca65c5")]
        [RoleId("724e2645-553a-4810-a62d-4c7595877042")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public TimeFrequency BillingFrequency { get; set; }

        #region Allors
        [Id("c163457c-6a36-45ab-8c62-e555128afbfc")]
        [AssociationId("01112e75-888e-4dac-93e0-185afe6988af")]
        [RoleId("56c9d8a5-45d0-4bb4-8809-43740938b824")]
        #endregion
        [Derived]
        [Workspace]
        public decimal AmountOfTime { get; set; }

        #region Allors
        [Id("B53DFC71-1BEA-40BC-B3F9-02F4FEA0EC43")]
        [AssociationId("6388C1E5-DAA8-4247-A3C9-CA6B43A613EA")]
        [RoleId("C4DBD911-80E6-4953-B379-BD599BE88F04")]
        #endregion
        [Workspace]
        public decimal AssignedAmountOfTime { get; set; }

        #region Allors
        [Id("816719D2-8386-4D19-BF3F-D1AC9A6BFB4F")]
        [AssociationId("2EE8A8B9-DA9A-403B-B775-E2BA9742F466")]
        [RoleId("0C73DE02-5D81-4DC6-BDA8-C523D377CBE7")]
        #endregion
        [Workspace]
        public decimal BillableAmountOfTime { get; set; }

        #region Allors
        [Id("430F0646-64C9-40EA-89AE-A07A30AF85B4")]
        [AssociationId("0A85B6C8-8AE2-4052-8456-2659A124638D")]
        [RoleId("A92105BF-90BA-413D-99B1-7A53C44DC7BF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public TimeFrequency TimeFrequency { get; set; }

        #region Allors
        [Id("E1121F4A-5CE3-4966-9DF0-CCA9A7DCCEB6")]
        [AssociationId("CDFE81B9-948A-4FE9-B97C-BFC356AD06E5")]
        [RoleId("BB5A8989-EACB-4756-85A1-01B9313EA086")]
        #endregion
        [Required]
        [Derived]
        [Workspace]
        public decimal BillingAmount { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        public void DelegateAccess() { }

        #endregion
    }
}
