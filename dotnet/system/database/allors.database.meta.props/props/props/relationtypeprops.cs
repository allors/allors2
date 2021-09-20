// <copyright file="RelationTypeProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

using System;

namespace Allors.Database.Meta
{
    public sealed partial class RelationTypeProps : MetaObjectProps, IMetaIdentifiableObjectProps
    {
        private readonly IRelationTypeBase relationType;

        internal RelationTypeProps(IRelationTypeBase relationType) => this.relationType = relationType;

        public Guid Id => this.relationType.Id;

        public string Tag => this.relationType.Tag;

        public IAssociationType AssociationType => this.relationType.AssociationType;

        public IRoleType RoleType => this.relationType.RoleType;

        public Multiplicity Multiplicity => this.relationType.Multiplicity;

        public bool IsOneToOne => this.relationType.IsOneToOne;

        public bool IsOneToMany => this.relationType.IsOneToMany;

        public bool IsManyToOne => this.relationType.IsManyToOne;

        public bool IsManyToMany => this.relationType.IsManyToMany;

        public bool ExistExclusiveDatabaseClasses => this.relationType.ExistExclusiveDatabaseClasses;

        public bool IsIndexed => this.relationType.IsIndexed;

        public bool IsDerived => this.relationType.IsDerived;

        public string[] WorkspaceNames => this.relationType.WorkspaceNames;

        public string Name => this.relationType.Name;

        #region As
        protected override IMetaObjectBase AsMetaObject => this.relationType;
        #endregion
    }
}
