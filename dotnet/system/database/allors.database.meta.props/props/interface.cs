// <copyright file="Interface.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract partial class Interface : Composite, IInterfaceBase
    {
        private string[] derivedWorkspaceNames;

        private HashSet<ICompositeBase> derivedDirectSubtypes;

        private HashSet<ICompositeBase> derivedSubtypes;
        private HashSet<ICompositeBase> derivedDatabaseSubtypes;

        private HashSet<IClassBase> derivedClasses;

        private HashSet<IClassBase> derivedDatabaseClasses;

        private IClassBase derivedExclusiveClass;

        private Type clrType;

        private InterfaceProps props;

        internal Interface(IMetaPopulationBase metaPopulation, Guid id, string tag) : base(metaPopulation, id, tag) => metaPopulation.OnInterfaceCreated(this);

        public MetaPopulation M => (MetaPopulation)this.MetaPopulation;

        public InterfaceProps _ => this.props ??= new InterfaceProps(this);

        public override IEnumerable<string> WorkspaceNames
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedWorkspaceNames;
            }
        }

        public bool ExistClasses
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedClasses.Count > 0;
            }
        }

        public bool ExistSubtypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedSubtypes.Count > 0;
            }
        }

        public override bool ExistClass
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedClasses.Count > 0;
            }
        }

        /// <summary>
        /// Gets the subclasses.
        /// </summary>
        /// <value>The subclasses.</value>
        public override IEnumerable<IClassBase> Classes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedClasses;
            }
        }

        /// <summary>
        /// Gets the sub types.
        /// </summary>
        /// <value>The super types.</value>
        IEnumerable<IComposite> IInterface.Subtypes => this.Subtypes;
        public override IEnumerable<ICompositeBase> Subtypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedSubtypes;
            }
        }

        public override IEnumerable<ICompositeBase> DatabaseSubtypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedDatabaseSubtypes;
            }
        }

        public IEnumerable<Interface> Subinterfaces => this.Subtypes.OfType<Interface>();

        public override IClassBase ExclusiveClass
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedExclusiveClass;
            }
        }

        public override Type ClrType => this.clrType;

        /// <summary>
        /// Contains this concrete class.
        /// </summary>
        /// <param name="objectType">
        /// The concrete class.
        /// </param>
        /// <returns>
        /// True if this contains the concrete class.
        /// </returns>
        public override bool IsAssignableFrom(IComposite objectType)
        {
            this.MetaPopulation.Derive();
            return this.Equals(objectType) || this.derivedSubtypes.Contains(objectType);
        }

        public override void Bind(Dictionary<string, Type> typeByTypeName) => this.clrType = typeByTypeName[this.Name];

        public void DeriveWorkspaceNames() =>
            this.derivedWorkspaceNames = this
                .RoleTypes.SelectMany(v => v.RelationType.WorkspaceNames)
                .Union(this.AssociationTypes.SelectMany(v => v.RelationType.WorkspaceNames))
                .Union(this.MethodTypes.SelectMany(v => v.WorkspaceNames))
                .ToArray();

        /// <summary>
        /// Derive direct sub type derivations.
        /// </summary>
        /// <param name="directSubtypes">The direct super types.</param>
        public void DeriveDirectSubtypes(HashSet<ICompositeBase> directSubtypes)
        {
            directSubtypes.Clear();
            foreach (var inheritance in this.MetaPopulation.Inheritances.Where(inheritance => this.Equals(inheritance.Supertype)))
            {
                directSubtypes.Add(inheritance.Subtype);
            }

            this.derivedDirectSubtypes = new HashSet<ICompositeBase>(directSubtypes);
        }

        /// <summary>
        /// Derive subclasses.
        /// </summary>
        /// <param name="subClasses">The sub classes.</param>
        public void DeriveSubclasses(HashSet<IClassBase> subClasses)
        {
            subClasses.Clear();
            foreach (var subType in this.derivedSubtypes)
            {
                if (subType is IClass)
                {
                    subClasses.Add((Class)subType);
                }
            }

            this.derivedClasses = new HashSet<IClassBase>(subClasses);
            this.derivedDatabaseClasses = new HashSet<IClassBase>(subClasses.Where(v => v.Origin == Origin.Database));
        }

        /// <summary>
        /// Derive sub types.
        /// </summary>
        /// <param name="subTypes">The super types.</param>
        public void DeriveSubtypes(HashSet<ICompositeBase> subTypes)
        {
            subTypes.Clear();
            this.DeriveSubtypesRecursively(this, subTypes);

            this.derivedSubtypes = new HashSet<ICompositeBase>(subTypes);
            this.derivedDatabaseSubtypes = new HashSet<ICompositeBase>(subTypes.Where(v => v.Origin == Origin.Database));
        }

        /// <summary>
        /// Derive exclusive sub classes.
        /// </summary>
        public void DeriveExclusiveSubclass() => this.derivedExclusiveClass = this.derivedClasses.Count == 1 ? this.derivedClasses.First() : null;

        /// <summary>
        /// Derive super types recursively.
        /// </summary>
        /// <param name="type">The type .</param>
        /// <param name="subTypes">The super types.</param>
        public void DeriveSubtypesRecursively(IObjectTypeBase type, HashSet<ICompositeBase> subTypes)
        {
            foreach (var directSubtype in this.derivedDirectSubtypes)
            {
                if (!Equals(directSubtype, type))
                {
                    subTypes.Add(directSubtype);
                    if (directSubtype is IInterface)
                    {
                        ((Interface)directSubtype).DeriveSubtypesRecursively(this, subTypes);
                    }
                }
            }
        }

        public override IEnumerable<IClass> DatabaseClasses
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedDatabaseClasses;
            }
        }
    }
}
