// <copyright file="Interface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed partial class Interface : Composite, IInterface
    {
        private LazySet<Composite> derivedDirectSubtypes;

        private LazySet<Composite> derivedSubtypes;

        private LazySet<Class> derivedSubclasses;

        private Class derivedExclusiveSubclass;

        private Type clrType;

        internal Interface(MetaPopulation metaPopulation, Guid id)
            : base(metaPopulation)
        {
            this.Id = id;

            metaPopulation.OnInterfaceCreated(this);
        }

        #region Exist
        public bool ExistSubclasses
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedSubclasses.Count > 0;
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
                return this.derivedSubclasses.Count > 0;
            }
        }

        #endregion

        IEnumerable<IClass> IInterface.Subclasses => this.Subclasses;

        /// <summary>
        /// Gets the subclasses.
        /// </summary>
        /// <value>The subclasses.</value>
        public IEnumerable<Class> Subclasses
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedSubclasses;
            }
        }

        public override IEnumerable<Class> Classes => this.Subclasses;

        IEnumerable<IComposite> IInterface.Subtypes => this.Subtypes;

        /// <summary>
        /// Gets the sub types.
        /// </summary>
        /// <value>The super types.</value>
        public IEnumerable<Composite> Subtypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedSubtypes;
            }
        }

        public override Class ExclusiveSubclass
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedExclusiveSubclass;
            }
        }

        #region Contains

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

        #endregion

        /// <summary>
        /// Derive direct sub type derivations.
        /// </summary>
        /// <param name="directSubtypes">The direct super types.</param>
        internal void DeriveDirectSubtypes(HashSet<Composite> directSubtypes)
        {
            directSubtypes.Clear();
            foreach (var inheritance in this.MetaPopulation.Inheritances.Where(inheritance => this.Equals(inheritance.Supertype)))
            {
                directSubtypes.Add(inheritance.Subtype);
            }

            this.derivedDirectSubtypes = new LazySet<Composite>(directSubtypes);
        }

        /// <summary>
        /// Derive subclasses.
        /// </summary>
        /// <param name="subClasses">The sub classes.</param>
        internal void DeriveSubclasses(HashSet<Class> subClasses)
        {
            subClasses.Clear();
            foreach (var subType in this.derivedSubtypes)
            {
                if (subType is IClass)
                {
                    subClasses.Add((Class)subType);
                }
            }

            this.derivedSubclasses = new LazySet<Class>(subClasses);
        }

        /// <summary>
        /// Derive sub types.
        /// </summary>
        /// <param name="subTypes">The super types.</param>
        internal void DeriveSubtypes(HashSet<Composite> subTypes)
        {
            subTypes.Clear();
            this.DeriveSubtypesRecursively(this, subTypes);

            this.derivedSubtypes = new LazySet<Composite>(subTypes);
        }

        /// <summary>
        /// Derive exclusive sub classes.
        /// </summary>
        internal void DeriveExclusiveSubclass() => this.derivedExclusiveSubclass = this.derivedSubclasses.Count == 1 ? this.derivedSubclasses.First() : null;

        /// <summary>
        /// Derive super types recursively.
        /// </summary>
        /// <param name="type">The type .</param>
        /// <param name="subTypes">The super types.</param>
        private void DeriveSubtypesRecursively(ObjectType type, HashSet<Composite> subTypes)
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

        public override Type ClrType => this.clrType;

        internal void Bind(Dictionary<string, Type> typeByTypeName) => this.clrType = typeByTypeName[this.Name];
    }
}
