// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Singletons
    {
        public Singleton Instance => this.Session.GetSingleton();

        protected override void CorePrepare(Setup setup)
        {
            base.CorePrepare(setup);

            setup.AddDependency(this.ObjectType, M.Locale.ObjectType);
        }

        protected override void CoreSetup(Setup setup)
        {
            var singleton = new SingletonBuilder(this.Session).Build();

            singleton.DefaultLocale = new Locales(this.Session).EnglishGreatBritain;
        }
    }
}
