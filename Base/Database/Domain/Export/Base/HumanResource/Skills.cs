
// <copyright file="Skills.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Skills
    {
        public static readonly Guid ProjectManagementId = new Guid("A8E4E325-8B5C-4f86-AB8E-A3D16C9B7827");

        private UniquelyIdentifiableSticky<Skill> cache;

        public Skill ProjectManagement => this.Cache[ProjectManagementId];

        private UniquelyIdentifiableSticky<Skill> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<Skill>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SkillBuilder(this.Session)
                .WithName("Project Management")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project Management").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProjectManagementId)
                .WithIsActive(true)
                .Build();
        }
    }
}
