// <copyright file="EUSalesListTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
