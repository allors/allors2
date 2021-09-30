// <copyright file="RelationTypeProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Database.Meta
{
    public abstract partial class MetaObjectProps
    {
        public const Origin DefaultOrigin = Origin.Database;

        public IMetaPopulationBase MetaPopulation => this.AsMetaObject.MetaPopulation;

        public Origin Origin => this.AsMetaObject.Origin;

        public int OriginAsInt => (int)this.Origin;

        public bool HasDatabaseOrigin => this.Origin == Origin.Database;

        public bool HasWorkspaceOrigin => this.Origin == Origin.Workspace;

        public bool HasSessionOrigin => this.Origin == Origin.Session;

        public string ValidationName => this.AsMetaObject.ValidationName;

        public bool IsDefaultOrigin => this.Origin == DefaultOrigin;

        #region As
        protected abstract IMetaObjectBase AsMetaObject { get; }
        #endregion
    }
}
