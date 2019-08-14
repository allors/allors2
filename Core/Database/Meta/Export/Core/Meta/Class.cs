//------------------------------------------------------------------------------------------------- 
// <copyright file="Class.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the IObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed partial class Class : Composite, IClass
    {
        // TODO: Review
        public RoleType[] DelegatedAccessRoleTypes { get; set; }

        private readonly Class[] classes;

        private readonly Dictionary<RoleType, ConcreteRoleType> concreteRoleTypeByRoleType;

        private readonly Dictionary<MethodType, ConcreteMethodType> concreteMethodTypeByMethodType;

        private ConcreteRoleType[] concreteRoleTypes;

        private ConcreteMethodType[] concreteMethodTypes;

        private Type clrType;

        internal Class(MetaPopulation metaPopulation, Guid id)
            : base(metaPopulation)
        {
            this.Id = id;

            this.concreteRoleTypeByRoleType = new Dictionary<RoleType, ConcreteRoleType>();
            this.concreteMethodTypeByMethodType = new Dictionary<MethodType, ConcreteMethodType>();

            this.classes = new[] { this };
            metaPopulation.OnClassCreated(this);
        }

        public Dictionary<RoleType, ConcreteRoleType> ConcreteRoleTypeByRoleType
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.concreteRoleTypeByRoleType;
            }
        }

        public Dictionary<MethodType, ConcreteMethodType> ConcreteMethodTypeByMethodType
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.concreteMethodTypeByMethodType;
            }
        }

        public ConcreteRoleType[] ConcreteRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.concreteRoleTypes;
            }
        }


        public ConcreteMethodType[] ConcreteMethodTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.concreteMethodTypes;
            }
        }

        public override IEnumerable<Class> Classes => this.classes;

        public override bool ExistClass => true;

        public override Class ExclusiveSubclass => this;
        
        public override Type ClrType => this.clrType;

        public IEnumerable<ConcreteRoleType> WorkspaceConcreteRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.ConcreteRoleTypes.Where(m => m.RoleType.Workspace);
            }
        }

        public IEnumerable<ConcreteMethodType> WorkspaceConcreteMethodTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.ConcreteMethodTypes.Where(m => m.MethodType.Workspace);
            }
        }

        public override IEnumerable<Composite> WorkspaceSubtypes => this.Workspace ? Array.Empty<Composite>() : new[] { this };

        public override bool IsAssignableFrom(IComposite objectType)
        {
            return this.Equals(objectType);
        }
       
        public void DeriveConcreteRoleTypes(HashSet<RoleType> sharedRoleTypes)
        {
            sharedRoleTypes.Clear();
            var removedRoleTypes = sharedRoleTypes;
            removedRoleTypes.UnionWith(this.ConcreteRoleTypeByRoleType.Keys);

            foreach (var roleType in this.RoleTypes)
            {
                removedRoleTypes.Remove(roleType);

                ConcreteRoleType concreteRoleType;
                if (!this.concreteRoleTypeByRoleType.TryGetValue(roleType, out concreteRoleType))
                {
                    concreteRoleType = new ConcreteRoleType(this, roleType);
                    this.concreteRoleTypeByRoleType[roleType] = concreteRoleType;
                }
            }

            foreach (var roleType in removedRoleTypes)
            {
                this.concreteRoleTypeByRoleType.Remove(roleType);
            }

            this.concreteRoleTypes = this.concreteRoleTypeByRoleType.Values.ToArray();
        }

        public void DeriveConcreteMethodTypes(HashSet<MethodType> sharedMethodTypes)
        {
            sharedMethodTypes.Clear();
            var removedMethodTypes = sharedMethodTypes;
            removedMethodTypes.UnionWith(this.concreteMethodTypeByMethodType.Keys);

            foreach (var methodType in this.MethodTypes)
            {
                removedMethodTypes.Remove(methodType);

                ConcreteMethodType concreteMethodType;
                if (!this.concreteMethodTypeByMethodType.TryGetValue(methodType, out concreteMethodType))
                {
                    concreteMethodType = new ConcreteMethodType(this, methodType);
                    this.concreteMethodTypeByMethodType[methodType] = concreteMethodType;
                }
            }

            foreach (var methodType in removedMethodTypes)
            {
                this.concreteMethodTypeByMethodType.Remove(methodType);
            }

            this.concreteMethodTypes = this.concreteMethodTypeByMethodType.Values.ToArray();
        }

        internal void Bind(Dictionary<string, Type> typeByTypeName)
        {
            this.clrType = typeByTypeName[this.Name];
        }
    }
}