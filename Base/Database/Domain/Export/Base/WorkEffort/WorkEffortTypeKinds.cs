
// <copyright file="WorkEffortTypeKinds.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class WorkEffortTypeKinds
    {
        private static readonly Guid ProgramId = new Guid("61480275-3EB4-45b1-9E2D-533ABB3A6C64");
        private static readonly Guid ProjectId = new Guid("4DFB282D-19A0-491b-B01D-16569E20B4F4");
        private static readonly Guid PhaseId = new Guid("6A73AE6F-1C74-4ad0-8784-CBEAEF3B62D5");
        private static readonly Guid ActivityId = new Guid("92E613F6-A12D-4BE1-9350-3F4C0DBA2EA1");
        private static readonly Guid WorkTaskId = new Guid("13472733-E7E9-47AC-A977-A8571BED05EC");

        private UniquelyIdentifiableSticky<WorkEffortTypeKind> cache;

        public WorkEffortTypeKind Program => this.Cache[ProgramId];

        public WorkEffortTypeKind Project => this.Cache[ProjectId];

        public WorkEffortTypeKind Phase => this.Cache[PhaseId];

        public WorkEffortTypeKind Activity => this.Cache[ActivityId];

        public WorkEffortTypeKind WorkTask => this.Cache[WorkTaskId];

        private UniquelyIdentifiableSticky<WorkEffortTypeKind> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<WorkEffortTypeKind>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new WorkEffortTypeKindBuilder(this.Session)
                .WithName("Program")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Programma").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProgramId)
                .WithIsActive(true)
                .Build();

            new WorkEffortTypeKindBuilder(this.Session)
                .WithName("Project")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProjectId)
                .WithIsActive(true)
                .Build();

            new WorkEffortTypeKindBuilder(this.Session)
                .WithName("Phase")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fase").WithLocale(dutchLocale).Build())
                .WithUniqueId(PhaseId)
                .WithIsActive(true)
                .Build();

            new WorkEffortTypeKindBuilder(this.Session)
                .WithName("Activity")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Activiteit").WithLocale(dutchLocale).Build())
                .WithUniqueId(ActivityId)
                .WithIsActive(true)
                .Build();

            new WorkEffortTypeKindBuilder(this.Session)
                .WithName("Task")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Taak").WithLocale(dutchLocale).Build())
                .WithUniqueId(WorkTaskId)
                .WithIsActive(true)
                .Build();
        }
    }
}
