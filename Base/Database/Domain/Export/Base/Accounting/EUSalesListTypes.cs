// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EuSalesListTypes.cs" company="Allors bvba">
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

    public partial class EuSalesListTypes
    {
        private static readonly Guid GoodsId = new Guid("1C5FDDAA-8BF8-4D28-829F-4D2012907556");
        private static readonly Guid ServicesId = new Guid("2166066E-3953-448D-AB0A-F3B96A307503");
        private static readonly Guid TriangularTradeId = new Guid("EA231348-081E-4867-B5D7-B85055EF40FB");

        private UniquelyIdentifiableSticky<EuSalesListType> cache;

        public EuSalesListType Goods => this.Cache[GoodsId];

        public EuSalesListType Services => this.Cache[ServicesId];

        public EuSalesListType TriangularTrade => this.Cache[TriangularTradeId];

        private UniquelyIdentifiableSticky<EuSalesListType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<EuSalesListType>(this.Session));

        protected override void BaseSetup(Setup setup)
        {


            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EuSalesListTypeBuilder(this.Session)
                .WithName("Goods")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goederen").WithLocale(dutchLocale).Build())
                .WithIsActive(true)
                .WithUniqueId(GoodsId).Build();

            new EuSalesListTypeBuilder(this.Session)
                .WithName("Services")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Diensten").WithLocale(dutchLocale).Build())
                .WithIsActive(true)
                .WithUniqueId(ServicesId).Build();

            new EuSalesListTypeBuilder(this.Session)
                .WithName("Triangular trade")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("ABC-transactie").WithLocale(dutchLocale).Build())
                .WithIsActive(true)
                .WithUniqueId(TriangularTradeId).Build();
        }
    }
}