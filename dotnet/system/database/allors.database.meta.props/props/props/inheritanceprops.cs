// <copyright file="InheritanceProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Database.Meta
{
    public sealed partial class InheritanceProps : MetaObjectProps
    {
        private readonly IInheritanceBase inheritance;

        internal InheritanceProps(IInheritanceBase @class) => this.inheritance = @class;

        public IInterface Supertype => this.inheritance.Supertype;

        public IComposite Subtype => this.inheritance.Subtype;

        #region As
        protected override IMetaObjectBase AsMetaObject => this.inheritance;
        #endregion
    }
}
