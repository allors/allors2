// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Test.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.General
{
    using System.Collections;
    using System.IO;
    using System.Xml;

    using Allors;
    using Allors.Adapters;
    using Allors.Meta;

    public abstract class Test
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

        public abstract Allors1.Meta.Domain GetMetaDomain();

        public abstract Allors1.Meta.Domain GetMetaDomain2();

        public abstract IDatabase GetPopulation();

        public abstract IDatabase GetPopulation2();

        public abstract ISession GetSession();

        public abstract IDatabaseSession GetSession2();

        public virtual ObjectType GetMetaType(IObject allorsObject)
        {
            return allorsObject.Strategy.ObjectType;
        }

        public RelationType[] GetOne2OneRelations(Allors1.Meta.Domain domain)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in domain.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsComposite && metaRelation.IsOneToOne)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RelationType[] GetOne2ManyRelations(Allors1.Meta.Domain domain)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in domain.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsComposite && metaRelation.IsOneToMany)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RelationType[] GetMany2OneRelations(Allors1.Meta.Domain domain)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in domain.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsComposite && metaRelation.IsManyToOne)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RelationType[] GetMany2ManyRelations(Allors1.Meta.Domain domain)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in domain.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsComposite && metaRelation.IsManyToMany)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RoleType[] GetBinaryRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsBinary)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetBooleanRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsBoolean)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetDateRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsDate)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetDateTimeRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsDateTime)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetDecimalRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsDecimal)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetDoubleRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsDouble)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetIntegerRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsInteger)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetLongRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsLong)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetStringRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsString)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RoleType[] GetUniqueRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsUnique)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public RelationType[] GetUnitRelations(Allors1.Meta.Domain domain)
        {
            var relations = new ArrayList();
            foreach (var metaRelation in domain.RelationTypes)
            {
                if (metaRelation.RoleType.ObjectType.IsUnit)
                {
                    relations.Add(metaRelation);
                }
            }

            return (RelationType[])relations.ToArray(typeof(RelationType));
        }

        public RoleType[] GetUnitRoles(ObjectType type)
        {
            var roleList = new ArrayList();
            foreach (var metaRole in type.RoleTypes)
            {
                if (metaRole.ObjectType.IsUnit)
                {
                    roleList.Add(metaRole);
                }
            }

            return (RoleType[])roleList.ToArray(typeof(RoleType));
        }

        public void Load(ISession session, string xml)
        {
            using (var stringReader = new StringReader(xml))
            {
                var reader = new XmlTextReader(stringReader);
                session.Population.Load(reader);
                reader.Close();
            }
        }
        
        protected string Save(ISession session)
        {
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlTextWriter(stringWriter);
                session.Population.Save(writer);
                writer.Close();
                return stringWriter.ToString();
            }
        }
    }
}