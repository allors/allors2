namespace Allors.Domain
{
    using System;

    using Allors.Meta;

    public partial class Settingses
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.Singleton);
        }

        protected override void AppsSetup(Setup setup)
        {
            var settings = new SettingsBuilder(this.Session)
                .WithSkuCounter(new CounterBuilder(this.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build())
                .WithPreferredCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .Build();

            this.Session.GetSingleton().Settings = settings;
        }
    }
}
