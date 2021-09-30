// <copyright file="RelationTypeProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Database.Meta
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed partial class ClassProps : CompositeProps
    {
        private readonly IClassBase @class;

        public IEnumerable<IRoleType> OverriddenRequiredRoleTypes => this.@class.OverriddenRequiredRoleTypes;

        public IEnumerable<IRoleType> OverriddenUniqueRoleTypes => this.@class.OverriddenUniqueRoleTypes;

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceOverriddenRequiredByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.OverriddenRequiredRoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceOverriddenUniqueByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.OverriddenUniqueRoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        internal ClassProps(IClassBase @class) => this.@class = @class;

        #region As
        protected override IMetaObjectBase AsMetaObject => this.@class;

        protected override IObjectTypeBase AsObjectType => this.@class;

        protected override ICompositeBase AsComposite => this.@class;
        #endregion
    }
}
