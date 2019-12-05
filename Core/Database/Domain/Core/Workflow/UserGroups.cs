// <copyright file="UserGroups.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Domain
{
    using System;
    using Allors;

    public partial class UserGroups
    {
        public static readonly Guid AdministratorsId = new Guid("CDC04209-683B-429C-BED2-440851F430DF");
        public static readonly Guid CreatorsId = new Guid("F0D8132B-79D6-4A30-A866-EF6E5C952761");

        private UniquelyIdentifiableSticky<UserGroup> cache;

        public UserGroup Administrators => this.Cache[AdministratorsId];

        public UserGroup Creators => this.Cache[CreatorsId];

        private UniquelyIdentifiableSticky<UserGroup> Cache => this.cache ??= new UniquelyIdentifiableSticky<UserGroup>(this.Session);

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(AdministratorsId, v => v.Name = "Administrators");
            merge(CreatorsId, v => v.Name = "Creators");
        }
    }
}
