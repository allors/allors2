// <copyright file="Class.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public abstract partial class Class : Composite, IClassBase
    {
        private string[] assignedWorkspaceNames;
        private string[] derivedWorkspaceNames;

        private IRoleType[] overriddenRequiredRoleTypes;
        private IRoleType[] overriddenUniqueRoleTypes;

        private IRoleType[] derivedRequiredRoleTypes;
        private IRoleType[] derivedUniqueRoleTypes;

        private readonly Class[] classes;
        private Type clrType;

        private ClassProps props;
        private ConcurrentDictionary<IMethodType, Action<object, object>[]> actionsByMethodType;

        internal Class(IMetaPopulationBase metaPopulation, Guid id, string tag) : base(metaPopulation, id, tag)
        {
            this.classes = new[] { this };
            metaPopulation.OnClassCreated(this);
        }

        public ClassProps _ => this.props ??= new ClassProps(this);

        public IRoleType[] OverriddenRequiredRoleTypes
        {
            get => this.overriddenRequiredRoleTypes ?? Array.Empty<IRoleType>();

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.overriddenRequiredRoleTypes = value;
                this.MetaPopulation.Stale();
            }
        }

        public IRoleType[] OverriddenUniqueRoleTypes
        {
            get => this.overriddenUniqueRoleTypes ?? Array.Empty<IRoleType>();

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.overriddenUniqueRoleTypes = value;
                this.MetaPopulation.Stale();
            }
        }

        public IRoleType[] RequiredRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedRequiredRoleTypes;
            }
        }

        public IRoleType[] UniqueRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedUniqueRoleTypes;
            }
        }

        public string[] AssignedWorkspaceNames
        {
            get => this.assignedWorkspaceNames;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedWorkspaceNames = value;
                this.MetaPopulation.Stale();
            }
        }

        public override IEnumerable<string> WorkspaceNames
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedWorkspaceNames;
            }
        }

        // TODO: Review
        public RoleType[] DelegatedAccessRoleTypes { get; set; }

        public override IEnumerable<IClassBase> Classes => this.classes;

        public override IEnumerable<IClass> DatabaseClasses => this.Origin == Origin.Database ? this.classes : Array.Empty<Class>();

        public override bool ExistClass => true;

        public override IClassBase ExclusiveClass => this;

        public override Type ClrType => this.clrType;

        public override IEnumerable<ICompositeBase> Subtypes => Array.Empty<ICompositeBase>();

        public override IEnumerable<ICompositeBase> DatabaseSubtypes => this.Origin == Origin.Database ? this.Subtypes : Array.Empty<Composite>();

        public void DeriveWorkspaceNames(HashSet<string> workspaceNames)
        {
            this.derivedWorkspaceNames = this.assignedWorkspaceNames ?? Array.Empty<string>();
            workspaceNames.UnionWith(this.derivedWorkspaceNames);
        }

        public void DeriveRequiredRoleTypes() =>
            this.derivedRequiredRoleTypes = this.RoleTypes
                .Where(v => v.IsRequired)
                .Union(this.OverriddenRequiredRoleTypes).ToArray();

        public void DeriveUniqueRoleTypes() =>
            this.derivedUniqueRoleTypes = this.RoleTypes
                .Where(v => v.IsUnique)
                .Union(this.OverriddenUniqueRoleTypes).ToArray();

        public override bool IsAssignableFrom(IComposite objectType) => this.Equals(objectType);

        public override void Bind(Dictionary<string, Type> typeByTypeName) => this.clrType = typeByTypeName[this.Name];

        public Action<object, object>[] Actions(IMethodType methodType)
        {
            this.actionsByMethodType ??= new ConcurrentDictionary<IMethodType, Action<object, object>[]>();
            if (!this.actionsByMethodType.TryGetValue(methodType, out var actions))
            {
                actions = this.MetaPopulation.MethodCompiler.Compile(this, methodType);
                this.actionsByMethodType[methodType] = actions;
            }

            return actions;
        }
    }
}
