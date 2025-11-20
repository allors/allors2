// <copyright file="Singletons.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class SecurityTokens
    {
        public static readonly Guid InitialSecurityTokenId = new Guid("BE3404FF-1FF1-4C26-935F-777DC0AF983C");
        public static readonly Guid DefaultSecurityTokenId = new Guid("EF20E782-0BFB-4C59-B9EB-DC502C2256CA");
        public static readonly Guid AdministratorSecurityTokenId = new Guid("8C7FE74E-A769-49FC-BF69-549DBABD55D8");

        private UniquelyIdentifiableSticky<SecurityToken> cache;

        public SecurityToken InitialSecurityToken => this.Cache[InitialSecurityTokenId];

        public SecurityToken DefaultSecurityToken => this.Cache[DefaultSecurityTokenId];

        public SecurityToken AdministratorSecurityToken => this.Cache[AdministratorSecurityTokenId];

        private UniquelyIdentifiableSticky<SecurityToken> Cache => this.cache ??= new UniquelyIdentifiableSticky<SecurityToken>(this.Session);

        protected override void CorePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.AccessControl.ObjectType);

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            var accessControls = new AccessControls(this.Session);

            merge(InitialSecurityTokenId, v =>
            {
                if (setup.Config.SetupSecurity)
                {
                    v.AddAccessControl(accessControls.Creators);
                    v.AddAccessControl(accessControls.GuestCreator);
                }
            });

            merge(DefaultSecurityTokenId, v =>
            {
                if (setup.Config.SetupSecurity)
                {
                    v.AddAccessControl(accessControls.Administrator);
                    v.AddAccessControl(accessControls.Guest);
                }
            });

            merge(AdministratorSecurityTokenId, v =>
            {
                if (setup.Config.SetupSecurity)
                {
                    v.AddAccessControl(accessControls.Administrator);
                }
            });
        }
    }
}
