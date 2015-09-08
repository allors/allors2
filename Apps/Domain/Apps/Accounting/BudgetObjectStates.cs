// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BudgetObjectStates.cs" company="Allors bvba">
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

    public partial class BudgetObjectStates
    {
        public static readonly Guid OpenedId = new Guid("D5DE64D5-FE6B-456D-81BE-10BAA8C75C89");
        public static readonly Guid ClosedId = new Guid("4986E755-51D6-4D88-86A4-F22445029D84");
        public static readonly Guid ReopenedId = new Guid("1C435A55-9327-4B32-AE62-07378B11CE0A");

        private UniquelyIdentifiableCache<BudgetObjectState> cache;

        public BudgetObjectState Opened
        {
            get { return this.Cache.Get(OpenedId); }
        }

        public BudgetObjectState Closed
        {
            get { return this.Cache.Get(ClosedId); }
        }

        private UniquelyIdentifiableCache<BudgetObjectState> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<BudgetObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new BudgetObjectStateBuilder(Session)
                .WithUniqueId(OpenedId)
                .WithName("Open")
                .Build();

            new BudgetObjectStateBuilder(Session)
                .WithUniqueId(ClosedId)
                .WithName("Closed")
                .Build();

            new BudgetObjectStateBuilder(Session)
                .WithUniqueId(ReopenedId)
                .WithName("Reopened")
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