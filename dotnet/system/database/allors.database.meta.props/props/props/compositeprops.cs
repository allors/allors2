// <copyright file="MethodClassProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MethodClass type.</summary>

namespace Allors.Database.Meta
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class CompositeProps : ObjectTypeProps
    {
        public bool ExistDirectSupertypes => this.DirectSupertypes.Any();

        public bool ExistSupertypes => this.Supertypes.Any();

        public bool ExistAssociationTypes => this.AssociationTypes.Any();

        public bool ExistRoleTypes => this.AssociationTypes.Any();

        public bool ExistMethodTypes => this.MethodTypes.Any();

        public IEnumerable<IInterface> Supertypes => this.AsComposite.Supertypes;

        public IEnumerable<IClass> Classes => this.AsComposite.Classes;

        public bool ExistExclusiveClass => this.AsComposite.ExistExclusiveClass;

        public IEnumerable<IInterface> DirectSupertypes => this.AsComposite.DirectSupertypes;

        public IEnumerable<IComposite> Subtypes => this.AsComposite.Subtypes;

        public IEnumerable<IComposite> RelatedComposites =>
            this
                .Supertypes
                .Union(this.RoleTypes.Where(m => m.ObjectType.IsComposite).Select(v => (IComposite)v.ObjectType))
                .Union(this.AssociationTypes.Select(v => v.ObjectType)).Distinct()
                .Except(new[] { this.AsComposite }).ToArray();

        public IEnumerable<IAssociationType> AssociationTypes => this.AsComposite.AssociationTypes;

        public IEnumerable<IAssociationType> ExclusiveAssociationTypes => this.AsComposite.ExclusiveAssociationTypes;

        public IEnumerable<IAssociationType> InheritedAssociationTypes => this.AsComposite.InheritedAssociationTypes;

        public IEnumerable<IRoleType> RoleTypes => this.AsComposite.RoleTypes;

        public IEnumerable<IRoleType> ExclusiveRoleTypes => this.AsComposite.ExclusiveRoleTypes;

        public IEnumerable<IRoleType> InheritedRoleTypes => this.AsComposite.InheritedRoleTypes;

        public IEnumerable<IAssociationType> DatabaseAssociationTypes => this.AsComposite.DatabaseAssociationTypes;

        public IEnumerable<IAssociationType> ExclusiveDatabaseAssociationTypes => this.AsComposite.ExclusiveDatabaseAssociationTypes;

        public IEnumerable<IRoleType> DatabaseRoleTypes => this.AsComposite.DatabaseRoleTypes;

        public IEnumerable<IRoleType> ExclusiveDatabaseRoleTypes => this.AsComposite.ExclusiveDatabaseRoleTypes;

        public IEnumerable<IMethodType> MethodTypes => this.AsComposite.MethodTypes;

        public IEnumerable<IMethodType> InheritedMethodTypes => this.AsComposite.InheritedMethodTypes;

        public IEnumerable<IMethodType> ExclusiveMethodTypes => this.AsComposite.ExclusiveMethodTypes;

        public bool ExistDatabaseClass => this.AsComposite.ExistDatabaseClass;

        public IEnumerable<IClass> DatabaseClasses => this.AsComposite.DatabaseClasses;

        public bool ExistExclusiveDatabaseClass => this.AsComposite.ExistExclusiveDatabaseClass;

        public IClass ExclusiveDatabaseClass => this.AsComposite.ExclusiveDatabaseClass;

        public IReadOnlyDictionary<string, IOrderedEnumerable<IAssociationType>> WorkspaceAssociationTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.AssociationTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IAssociationType>> WorkspaceExclusiveAssociationTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.ExclusiveAssociationTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IAssociationType>> WorkspaceInheritedAssociationTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.InheritedAssociationTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceRoleTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.RoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceCompositeRoleTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.RoleTypes.Where(w => w.ObjectType.IsComposite && w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceInheritedRoleTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.InheritedRoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceExclusiveRoleTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.ExclusiveRoleTypes.Where(w => w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceExclusiveRoleTypesWithDatabaseOriginByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.ExclusiveRoleTypes.Where(w => w.Origin == Origin.Database && w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceExclusiveRoleTypesWithWorkspaceOrSessionOriginByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.ExclusiveRoleTypes.Where(w => (w.Origin == Origin.Workspace || w.Origin == Origin.Session) && w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IRoleType>> WorkspaceExclusiveCompositeRoleTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v,
                        v => this.ExclusiveRoleTypes.Where(w => w.ObjectType.IsComposite && w.RelationType.WorkspaceNames.Contains(v)).OrderBy(w => w.RelationType.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IMethodType>> WorkspaceExclusiveMethodTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.ExclusiveMethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IMethodType>> WorkspaceInheritedMethodTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.InheritedMethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IMethodType>> WorkspaceMethodTypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.MethodTypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IInterface>> WorkspaceDirectSupertypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.DirectSupertypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IInterface>> WorkspaceSupertypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.Supertypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IComposite>> WorkspaceSubtypesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.Subtypes.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));
            }
        }

        public IReadOnlyDictionary<string, IOrderedEnumerable<IComposite>> WorkspaceRelatedCompositesByWorkspaceName
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.WorkspaceNames
                    .ToDictionary(v => v, v => this.RelatedComposites.Where(w => w.WorkspaceNames.Contains(v)).OrderBy(w => w.Tag));
            }
        }

        #region As
        protected abstract ICompositeBase AsComposite { get; }
        #endregion
    }
}
