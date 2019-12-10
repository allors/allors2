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

        private UniquelyIdentifiableSticky<WorkEffortTypeKind> Cache => this.cache ??= new UniquelyIdentifiableSticky<WorkEffortTypeKind>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(ProgramId, v =>
            {
                v.Name = "Program";
                localisedName.Set(v, dutchLocale, "Programma");
                v.IsActive = true;
            });

            merge(ProjectId, v =>
            {
                v.Name = "Project";
                localisedName.Set(v, dutchLocale, "Project");
                v.IsActive = true;
            });

            merge(PhaseId, v =>
            {
                v.Name = "Phase";
                localisedName.Set(v, dutchLocale, "Fase");
                v.IsActive = true;
            });

            merge(ActivityId, v =>
            {
                v.Name = "Activity";
                localisedName.Set(v, dutchLocale, "Activiteit");
                v.IsActive = true;
            });

            merge(WorkTaskId, v =>
            {
                v.Name = "Task";
                localisedName.Set(v, dutchLocale, "Taak");
                v.IsActive = true;
            });
        }
    }
}
