// <copyright file="Settingses.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    using Allors.Meta;

    public partial class Settingses
    {
        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.Singleton);
            setup.AddDependency(this.ObjectType, M.InventoryStrategy);
            setup.AddDependency(this.ObjectType, M.Currency);
        }

        protected override void BaseSetup(Setup setup)
        {
            var singleton = this.Session.GetSingleton();
            singleton.Settings ??= new SettingsBuilder(this.Session)
                .WithUseProductNumberCounter(true)
                .WithUsePartNumberCounter(true)
                .Build();

            var settings = singleton.Settings;

            var inventoryStrategy = new InventoryStrategies(this.Session).Standard;
            var preferredCurrency = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            settings.InventoryStrategy ??= inventoryStrategy;
            settings.SkuPrefix ??= "Sku";
            settings.SerialisedItemPrefix ??= "S";
            settings.ProductNumberPrefix ??= "art-";
            settings.PartNumberPrefix ??= "part-";
            settings.PreferredCurrency ??= preferredCurrency;

            settings.SkuCounter ??= new CounterBuilder(this.Session).Build();
            settings.SerialisedItemCounter ??= new CounterBuilder(this.Session).Build();
            settings.ProductNumberCounter ??= new CounterBuilder(this.Session).Build();
            settings.PartNumberCounter ??= new CounterBuilder(this.Session).Build();
        }
    }
}
