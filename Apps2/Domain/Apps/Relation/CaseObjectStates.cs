// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaseObjectStates.cs" company="Allors bvba">
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

    public partial class CaseObjectStates
    {
        private static readonly Guid ReadId = new Guid("595CD7D4-8CB5-463b-8661-8859B1A0484D");
        private static readonly Guid ClosedId = new Guid("F13E2DE5-32C0-4d6b-9949-F6D60B087A8A");
        private static readonly Guid InProgressId = new Guid("5C5B605F-ABF3-4956-A289-CA4AD3B3B4BE");
        private static readonly Guid CompletedId = new Guid("8203E84A-3299-448a-982E-4A79534CAB85");
        private static readonly Guid OpenedId = new Guid("4FF18EE3-C827-47a8-A5DE-EFA29CE9BB68");

        private UniquelyIdentifiableCache<CaseObjectState> stateCache;

        public CaseObjectState Opened => this.StateCache.Get(OpenedId);

        public CaseObjectState Closed => this.StateCache.Get(ClosedId);

        public CaseObjectState Read => this.StateCache.Get(ReadId);

        public CaseObjectState InProgress => this.StateCache.Get(InProgressId);

        public CaseObjectState Completed => this.StateCache.Get(CompletedId);

        private UniquelyIdentifiableCache<CaseObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<CaseObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new CaseObjectStateBuilder(this.Session)
                .WithUniqueId(ClosedId)
                .WithName("Closed")
                .Build();

            new CaseObjectStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new CaseObjectStateBuilder(this.Session)
                .WithUniqueId(InProgressId)
                .WithName("In Progress")
                .Build();

            new CaseObjectStateBuilder(this.Session)
                .WithUniqueId(OpenedId)
                .WithName("Open")
                .Build();

            new CaseObjectStateBuilder(this.Session)
                .WithUniqueId(ReadId)
                .WithName("Read")
                .Build();
        }
    }
}
