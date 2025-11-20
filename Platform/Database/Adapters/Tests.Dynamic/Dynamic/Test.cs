// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Test.cs" company="Allors bv">
//   Copyright Allors bv.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Allors;
    using Adapters;
    using Allors.Meta;

    public abstract class Test : IDisposable
    {
        protected readonly int ObjectsPerConcreteClass = 5;
        protected readonly TestValueGenerator ValueGenerator = new TestValueGenerator();
        private readonly bool[] boolFlags = { false, true };

        // Thorough
        // int[] repeats = { 1, 2 };
        // int[] testRepeats = { 1, 2 };
        // int[] assertRepeats = { 1, 2 };
        // private int objectsPerConcreteClass = 100;

        // Quick
        private readonly int[] repeats = { 1 };
        private readonly int[] testRepeats = { 1 };
        private readonly int[] assertRepeats = { 1 };

        public void Commit(bool transactionFlag)
        {
            if (transactionFlag)
            {
                this.GetSession().Commit();
            }
        }

        public void Rollback(bool rollbackFlag)
        {
            if (rollbackFlag)
            {
                this.GetSession().Rollback();
            }
        }

        public virtual int[] GetRepeats()
        {
            return this.repeats;
        }

        public virtual int[] GetAssertRepeats()
        {
            return this.assertRepeats;
        }

        public virtual int[] GetTestRepeats()
        {
            return this.testRepeats;
        }

        public abstract IObject[] CreateArray(ObjectType objectType, int count);

        public abstract IDatabase CreateMemoryPopulation();

        public virtual bool[] GetBooleanFlags()
        {
            return this.boolFlags;
        }

        public abstract bool IsRollbackSupported();

        public abstract MetaPopulation GetMetaPopulation();

        public abstract MetaPopulation GetMetaPopulation2();

        public abstract IDatabase GetPopulation();

        public abstract IDatabase GetPopulation2();

        public abstract ISession GetSession();

        public abstract ISession GetSession2();

        public virtual IClass GetMetaType(IObject allorsObject) => allorsObject.Strategy.Class;

        public RelationType[] GetOne2OneRelations(MetaPopulation metaPopulation)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in metaPopulation.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsComposite && metaRelation.IsOneToOne)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RelationType[] GetOne2ManyRelations(MetaPopulation metaPopulation)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in metaPopulation.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsComposite && metaRelation.IsOneToMany)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RelationType[] GetMany2OneRelations(MetaPopulation metaPopulation)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in metaPopulation.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsComposite && metaRelation.IsManyToOne)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RelationType[] GetMany2ManyRelations(MetaPopulation metaPopulation)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in metaPopulation.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsComposite && metaRelation.IsManyToMany)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public Class[] GetClasses(RelationType relationType) => ((Composite)relationType.RoleType.ObjectType).Classes.ToArray();
        
        public RelationType[] GetBinaryRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType is Unit unit && unit.IsBinary)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public RelationType[] GetBooleanRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType is Unit unit && unit.IsBoolean)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public RelationType[] GetDateTimeRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType is Unit unit && unit.IsDateTime)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public RelationType[] GetDecimalRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType is Unit unit && unit.IsDecimal)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public RelationType[] GetFloatRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType is Unit unit && unit.IsFloat)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public RelationType[] GetIntegerRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType is Unit unit && unit.IsInteger)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public RelationType[] GetStringRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType is Unit unit && unit.IsString)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public RelationType[] GetUniqueRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType is Unit unit && unit.IsUnique)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public RelationType[] GetUnitRelations(MetaPopulation metaPopulation)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in metaPopulation.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsUnit)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RelationType[] GetUnitRoles(Composite type)
        {
            var roleList = new List<RelationType>();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsUnit)
                {
                    roleList.Add(metaRole.RelationType);
                }
            }

            return roleList.ToArray();
        }

        public void Load(ISession session, string xml)
        {
            using (var stringReader = new StringReader(xml))
            {
                var reader = new XmlTextReader(stringReader);
                session.Database.Load(reader);
                reader.Close();
            }
        }

        protected string Save(ISession session)
        {
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlTextWriter(stringWriter);
                session.Database.Save(writer);
                writer.Close();
                return stringWriter.ToString();
            }
        }

        public abstract void Dispose();
    }
}
