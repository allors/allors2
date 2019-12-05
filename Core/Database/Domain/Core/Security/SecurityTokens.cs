// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SecurityTokens
    {
        public static readonly Guid InitialSecurityTokenId = new Guid("BE3404FF-1FF1-4C26-935F-777DC0AF983C");
        public static readonly Guid DefaultSecurityTokenId = new Guid("EF20E782-0BFB-4C59-B9EB-DC502C2256CA");

        private UniquelyIdentifiableSticky<SecurityToken> cache;

        public SecurityToken InitialSecurityToken => this.Cache[InitialSecurityTokenId];

        public SecurityToken DefaultSecurityToken => this.Cache[DefaultSecurityTokenId];

        private UniquelyIdentifiableSticky<SecurityToken> Cache => this.cache ??= new UniquelyIdentifiableSticky<SecurityToken>(this.Session);

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(InitialSecurityTokenId, v => { });
            merge(DefaultSecurityTokenId, v => { });
        }
    }
}
