// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Skills.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class Skills
    {
        public static readonly Guid ProjectManagementId = new Guid("A8E4E325-8B5C-4f86-AB8E-A3D16C9B7827");

        private UniquelyIdentifiableCache<Skill> cache;

        public Skill ProjectManagement
        {
            get { return this.Cache.Get(ProjectManagementId); }
        }

        private UniquelyIdentifiableCache<Skill> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<Skill>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SkillBuilder(this.Session)
                .WithName("Project Management")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project Management").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project Management").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProjectManagementId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
