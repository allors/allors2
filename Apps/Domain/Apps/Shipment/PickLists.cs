// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickLists.cs" company="Allors bvba">
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


    public partial class PickLists
    {
        public static readonly Guid PickListTemplateEnId = new Guid("3C1DC277-18B9-4d4e-8314-3719FAAF7DF7");
        public static readonly Guid PickListTemplateNlId = new Guid("F44DD4D4-B293-4307-A0F6-17528477B406");

        public Extent<PickList> PendingPickLists
        {
            get
            {
                var pickLists = new PickLists(this.Session).Extent();
                pickLists.Filter.AddNot().AddExists(PickLists.Meta.Picker);
                pickLists.Filter.AddEquals(PickLists.Meta.CurrentObjectState, new PickListObjectStates(this.Session).Created);

                return pickLists;
            }
        }

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, Domain.TemplatePurposes.Meta.ObjectType);
            setup.AddDependency(this.ObjectType, PickListObjectStates.Meta.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englischLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new StringTemplateBuilder(Session)
                .WithName("PickList " + englischLocale.Name)
                .WithBody(PickListTemplateEn)
                .WithUniqueId(PickListTemplateEnId)
                .WithLocale(englischLocale)
                .WithTemplatePurpose(new Domain.TemplatePurposes(this.Session).PickList)
                .Build();

            new StringTemplateBuilder(Session)
                .WithName("PickList " + dutchLocale.Name)
                .WithBody(PickListTemplateNl)
                .WithUniqueId(PickListTemplateNlId)
                .WithLocale(dutchLocale)
                .WithTemplatePurpose(new Domain.TemplatePurposes(this.Session).PickList)
                .Build();
        }

        protected override void AppsSecure(Domain.Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
            
            config.GrantOperations(this.ObjectType, full);

            config.GrantCustomer(this.ObjectType, Meta.CurrentPickListStatus, Operation.Read);
            config.GrantCustomer(this.ObjectType, Meta.PickListStatuses, Operation.Read);

            var created = new PickListObjectStates(Session).Created;
            var onHold = new PickListObjectStates(Session).OnHold;
            var picked = new PickListObjectStates(Session).Picked;
            var cancelled = new PickListObjectStates(Session).Cancelled;

            config.Deny(this.ObjectType, created, Meta.Continue);
            config.Deny(this.ObjectType, onHold, Meta.Hold);

            config.Deny(this.ObjectType, picked, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, cancelled, Operation.Execute, Operation.Write);
        }
    }
}