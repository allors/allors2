// <copyright file="DomainProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;

    public sealed partial class DomainProps : MetaObjectProps, IMetaIdentifiableObjectProps
    {
        private readonly IDomainBase domain;

        internal DomainProps(IDomainBase @class) => this.domain = @class;

        public Guid Id => this.domain.Id;

        public string Tag => this.domain.Tag;

        public string Name => this.domain.Name;

        public IEnumerable<IDomain> DirectSuperdomains => this.domain.DirectSuperdomains;

        #region As
        protected override IMetaObjectBase AsMetaObject => this.domain;
        #endregion
    }
}
