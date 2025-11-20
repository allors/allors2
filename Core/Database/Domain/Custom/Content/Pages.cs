// <copyright file="Two.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class Pages
    {
        public static readonly Guid IndexId = new Guid("A88D6A90-43F0-49B6-83D6-B05B2F783F9D");

        private UniquelyIdentifiableSticky<Page> cache;

        public Sticky<Guid, Page> Cache => this.cache ??= new UniquelyIdentifiableSticky<Page>(this.Session);

        public Page Index => this.Cache[IndexId];

        protected override void CustomPrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.Media.ObjectType);

        protected override void CustomSetup(Setup setup)
        {
            var medias = new Medias(this.Session);

            var merge = this.Cache.Merger().Action();

            merge(IndexId, v =>
            {
                v.Name = "About";
                v.Content = medias.About;
            });
        }
    }
}
