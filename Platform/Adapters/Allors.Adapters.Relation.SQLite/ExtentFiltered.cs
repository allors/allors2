//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsExtentFilteredSql.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the AllorsExtentFilteredSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SQLite
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;

    using Allors.Meta;

    public class AllorsExtentFilteredSql : AllorsExtentSql
    {
        internal readonly Session session;
        internal readonly IComposite type;
        internal IAssociationType association;
        private AllorsPredicateAndSql filter;
        internal IRoleType role;
        // IRelationTypes direct from Strategy
        internal Strategy strategy;

        internal AllorsExtentFilteredSql(Session session, Strategy strategy, IRoleType role) : this(session, (IComposite)role.ObjectType)
        {
            this.strategy = strategy;
            this.role = role;
        }

        internal AllorsExtentFilteredSql(Session session, Strategy strategy, IAssociationType association) : this(session, association.ObjectType)
        {
            this.strategy = strategy;
            this.association = association;
        }

        internal AllorsExtentFilteredSql(Session session, IComposite type)
        {
            this.session = session;
            this.type = type;
        }

        public override ICompositePredicate Filter
        {
            get
            {
                LazyLoadFilter();
                return filter;
            }
        }

        internal Mapping Mapping
        {
            get { return session.Database.Mapping; }
        }

        internal override Session Session
        {
            get { return session; }
        }

        public override IComposite ObjectType
        {
            get { return type; }
        }

        protected override ObjectId[] GetObjectIds()
        {
            if (this.strategy != null)
            {
                if (this.role != null)
                {
                    return this.strategy.ExtentGetCompositeRoles(this.role);
                }

                return this.strategy.ExtentGetCompositeAssociations(this.association);
            }

            if (!this.type.ExistClass)
            {
                return ObjectId.EmptyObjectIds;
            }
            
            this.session.Flush();

            var statement = new AllorsExtentStatementRootSql(this);
            var objects = new List<ObjectId>();

            this.BuildSql(statement);

            if (statement.Sorter != null)
            {
                statement.Sorter.BuildOrder(statement.Sorter, this.Mapping, statement);
            }

            using (var command = statement.CreateSQLiteCommand())
            {
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var objectId = new ObjectIdLong(reader.GetInt64(0));
                        objects.Add(objectId);
                    }

                    reader.Close();
                }
            }

            return objects.ToArray();
        }

        protected override void LazyLoadFilter()
        {
            if (this.filter == null)
            {
                this.filter = new AllorsPredicateAndSql(this);
                this.strategy = null;
                this.association = null;
                this.role = null;
                this.FlushCache();
            }
        }

        internal override string BuildSql(AllorsExtentStatementSql statement)
        {
            if (this.role != null || this.association != null)
            {
                // We're being used in another Extent so we need to
                // migrate from role/associatin to filter
                this.LazyLoadFilter();
            }

            if (this.filter != null)
            {
                this.filter.Setup(this, statement);
            }

            string alias = statement.CreateAlias();

            if (statement.IsRoot)
            {
                //TODO: DISTINCT isn't always necessary
                statement.Append("SELECT DISTINCT " + alias + "." + Mapping.ColumnNameForObject);

                if (statement.Sorter != null)
                {
                    statement.Sorter.Setup(this, statement);
                    statement.Sorter.BuildSelect(this, this.Mapping, statement);
                }

                statement.Append(" FROM " + Mapping.TableNameForObjects + " " + alias);

                statement.AddJoins(alias);
                statement.AddWhere(alias);

                if (this.filter != null)
                {
                    this.filter.BuildWhere(this, this.Mapping, statement, this.type, alias);
                }
            }
            else
            {
                var inStatement = (AllorsExtentStatementChildSql)statement;
                if (inStatement.Role != null)
                {
                    var inRole = inStatement.Role;
                    var inAssociation = inRole.AssociationType;
                    statement.Append("SELECT " + inAssociation.SingularFullName + "_A." + Mapping.ColumnNameForAssociation);
                }
                else
                {
                    var inAssociation = inStatement.Association;
                    var inRole = inAssociation.RoleType;
                    statement.Append("SELECT " + inRole.SingularFullName + "_R." + Mapping.ColumnNameForRole);
                }

                statement.Append(" FROM " + Mapping.TableNameForObjects + " " + alias);

                statement.AddJoins(alias);
                statement.AddWhere(alias);

                if (this.filter != null)
                {
                    this.filter.BuildWhere(this, this.Mapping, statement, this.type, alias);
                }

                statement.Append(" AND ");

                if (inStatement.Role != null)
                {
                    var inRole = inStatement.Role;
                    var inAssociation = inRole.AssociationType;
                    statement.Append(inAssociation.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL ");
                }
                else
                {
                    var inAssociation = inStatement.Association;
                    var inRole = inAssociation.RoleType;
                    statement.Append(inRole.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL ");
                }
            }

            return alias;
        }

        internal void CheckAssociation(IAssociationType associationType)
        {
            // TODO: Optimize
            if (!new List<IAssociationType>(this.type.AssociationTypes).Contains(associationType))
            {
                throw new ArgumentException("Extent does not implement association " + associationType.SingularFullName);
            }
        }

        internal void CheckRole(IRoleType roleType)
        {
            // TODO: Optimize
            if (!new List<IRoleType>(this.type.RoleTypes).Contains(roleType))
            {
                throw new ArgumentException("Extent does not implement role " + roleType.SingularFullName);
            }
        }
    }
}