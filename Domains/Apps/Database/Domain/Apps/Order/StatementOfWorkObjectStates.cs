// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQuoteObjectStates.cs" company="Allors bvba">
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

    public partial class StatementOfWorkObjectStates
    {
        private static readonly Guid CreatedId = new Guid("59754BD1-5DF8-42FA-B223-6F3382286684");
        private static readonly Guid ApprovedId = new Guid("ABD19A67-0901-4580-AF6B-64C92923EAC0");
        private static readonly Guid OrderedId = new Guid("0B89269B-DBD9-40B6-8AB4-F194E5B9ED30");
        private static readonly Guid RejectedId = new Guid("FDDB19E6-CB97-4827-861A-49EC0C54F7A0");

        private UniquelyIdentifiableCache<StatementOfWorkObjectState> stateCache;

        public StatementOfWorkObjectState Created => this.StateCache.Get(CreatedId);

        public StatementOfWorkObjectState Approved => this.StateCache.Get(ApprovedId);

        public StatementOfWorkObjectState Ordered => this.StateCache.Get(OrderedId);

        public StatementOfWorkObjectState Rejected => this.StateCache.Get(RejectedId);

        private UniquelyIdentifiableCache<StatementOfWorkObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<StatementOfWorkObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new StatementOfWorkObjectStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new StatementOfWorkObjectStateBuilder(this.Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new StatementOfWorkObjectStateBuilder(this.Session)
                .WithUniqueId(OrderedId)
                .WithName("Ordered")
                .Build();

            new StatementOfWorkObjectStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();
        }
    }
}