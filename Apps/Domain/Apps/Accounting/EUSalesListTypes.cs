// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EuSalesListTypes.cs" company="Allors bvba">
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

    public partial class EuSalesListTypes
    {
        public static readonly Guid GoodsId = new Guid("1C5FDDAA-8BF8-4D28-829F-4D2012907556");
        public static readonly Guid ServicesId = new Guid("2166066E-3953-448D-AB0A-F3B96A307503");
        public static readonly Guid TriangularTradeId = new Guid("EA231348-081E-4867-B5D7-B85055EF40FB");

        private UniquelyIdentifiableCache<EuSalesListType> cache;

        public EuSalesListType Goods
        {
            get { return this.Cache.Get(GoodsId); }
        }

        public EuSalesListType Services
        {
            get { return this.Cache.Get(ServicesId); }
        }

        public EuSalesListType TriangularTrade
        {
            get { return this.Cache.Get(TriangularTradeId); }
        }

        private UniquelyIdentifiableCache<EuSalesListType> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<EuSalesListType>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EuSalesListTypeBuilder(this.Session)
                .WithName("Goods")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goods").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goederen").WithLocale(dutchLocale).Build())
                .WithUniqueId(GoodsId).Build();

            new EuSalesListTypeBuilder(this.Session)
                .WithName("Services")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Services").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Diensten").WithLocale(dutchLocale).Build())
                .WithUniqueId(ServicesId).Build();

            new EuSalesListTypeBuilder(this.Session)
                .WithName("Triangular trade")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Triangular trade").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("ABC-transactie").WithLocale(dutchLocale).Build())
                .WithUniqueId(TriangularTradeId).Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}