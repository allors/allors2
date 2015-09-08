// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackagingSlips.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    public partial class PackagingSlips
    {
        public static readonly Guid PackagingSlipTemplateEnId = new Guid("528805EA-CCDE-41D7-9323-5E0638D75399");
        public static readonly Guid PackagingSlipTemplateNlId = new Guid("2E4C9D44-8CF2-4359-94B2-B619069F993F");

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, Domain.TemplatePurposes.Meta.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new StringTemplateBuilder(Session)
                .WithName("ShipmentPackage " + englishLocale.Name)
                .WithBody(PackagingSlipTemplateEn)
                .WithUniqueId(PackagingSlipTemplateEnId)
                .WithLocale(englishLocale)
                .WithTemplatePurpose(new Domain.TemplatePurposes(this.Session).PackagingSlip)
                .Build();

            new StringTemplateBuilder(Session)
                .WithName("ShipmentPackage " + dutchLocale.Name)
                .WithBody(PackagingSlipTemplateNl)
                .WithUniqueId(PackagingSlipTemplateNlId)
                .WithLocale(dutchLocale)
                .WithTemplatePurpose(new Domain.TemplatePurposes(this.Session).PackagingSlip)
                .Build();
        }

        protected override void AppsSecure(Domain.Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}