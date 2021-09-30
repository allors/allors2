// <copyright file="RelationTypeProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Database.Meta
{
    using System.Collections.Generic;

    public sealed partial class InterfaceProps : CompositeProps
    {
        private readonly IInterfaceBase @interface;

        internal InterfaceProps(IInterfaceBase @class) => this.@interface = @class;

        public IEnumerable<IComposite> Subtypes => this.@interface.Subtypes;

        #region As
        protected override IMetaObjectBase AsMetaObject => this.@interface;

        protected override IObjectTypeBase AsObjectType => this.@interface;

        protected override ICompositeBase AsComposite => this.@interface;
        #endregion
    }
}
