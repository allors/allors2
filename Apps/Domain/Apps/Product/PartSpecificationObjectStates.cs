// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartSpecificationObjectStates.cs" company="Allors bvba">
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

    public partial class PartSpecificationObjectStates
    {
        public static readonly Guid CreatedId = new Guid("89D6029D-2F90-43c7-989A-CF9EF7FE5DCE");
        public static readonly Guid DesignedId = new Guid("0B9F3E57-D3DE-43c7-AD71-0558D8CDF114");
        public static readonly Guid TestedId = new Guid("E87C46E1-D357-4cb8-B2DA-2BB7ADAF4970");
        public static readonly Guid ApprovedId = new Guid("B1FE3774-BD98-4230-9371-717F97DCD25B");
        public static readonly Guid RequirementSpecifiedId = new Guid("3604C076-EB3E-44c8-B855-D4F20918EC70");

        public const string ApproveIdString = "7E08EA40-5A3B-4a18-A675-97212BA896DE";

        private UniquelyIdentifiableCache<PartSpecificationObjectState> stateCache;

        public PartSpecificationObjectState Created
        {
            get { return this.StateCache.Get(CreatedId); }
        }

        public PartSpecificationObjectState Designed
        {
            get { return this.StateCache.Get(DesignedId); }
        }

        public PartSpecificationObjectState Tested
        {
            get { return this.StateCache.Get(TestedId); }
        }

        public PartSpecificationObjectState Approved
        {
            get { return this.StateCache.Get(ApprovedId); }
        }

        public PartSpecificationObjectState RequirementSpecified
        {
            get { return this.StateCache.Get(RequirementSpecifiedId); }
        }

        private UniquelyIdentifiableCache<PartSpecificationObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PartSpecificationObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishtLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new PartSpecificationObjectStateBuilder(Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PartSpecificationObjectStateBuilder(Session)
                .WithUniqueId(DesignedId)
                .WithName("Designed")
                .Build();

            new PartSpecificationObjectStateBuilder(Session)
                .WithUniqueId(TestedId)
                .WithName("Tested")
                .Build();

            new PartSpecificationObjectStateBuilder(Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new PartSpecificationObjectStateBuilder(Session)
                .WithUniqueId(RequirementSpecifiedId)
                .WithName("Requirement Specified")
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