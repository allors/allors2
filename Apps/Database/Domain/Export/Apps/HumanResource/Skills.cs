// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Skills.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class Skills
    {
        public static readonly Guid ProjectManagementId = new Guid("A8E4E325-8B5C-4f86-AB8E-A3D16C9B7827");

        private UniquelyIdentifiableSticky<Skill> cache;

        public Skill ProjectManagement => this.Cache[ProjectManagementId];

        private UniquelyIdentifiableSticky<Skill> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<Skill>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

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
