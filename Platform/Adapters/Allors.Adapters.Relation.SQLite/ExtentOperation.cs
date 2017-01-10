//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsExtentOperationSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsExtentOperationSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;

    using Allors.Meta;

    public enum AllorsExtentOperationTypeSqlBundled
    {
        UNION,
        INTERSECT,
        EXCEPT
    }

    public class AllorsExtentOperationSql : AllorsExtentSql
    {
        protected readonly AllorsExtentSql first;
        protected readonly AllorsExtentOperationTypeSqlBundled operationType;
        protected readonly AllorsExtentSql second;

        internal AllorsExtentOperationSql(AllorsExtentSql first, AllorsExtentSql second, AllorsExtentOperationTypeSqlBundled operationType)
        {
            if (!first.ObjectType.Equals(second.ObjectType))
            {
                throw new ArgumentException("Both extents in a Union, Intersect or Except must be from the same type");
            }

            if (first is AllorsExtentOperationSql || second is AllorsExtentOperationSql)
            {
                throw new NotSupportedException("Extent with operation can not be nested");
            }

            this.first = first;
            this.second = second;
            this.operationType = operationType;

            first.ParentOperationExtent = this;
            second.ParentOperationExtent = this;
        }

        public override ICompositePredicate Filter
        {
            get { return null; }
        }

        internal override Session Session
        {
            get { return this.first.Session; }
        }

        public override IComposite ObjectType
        {
            get { return this.first.ObjectType; }
        }

        public override Extent AddSort(IRoleType roleType)
        {
            return this.AddSort(roleType, SortDirection.Ascending);
        }

        protected override ObjectId[] GetObjectIds()
        {
            this.first.Session.Flush();

            var statement = new AllorsExtentStatementRootSql(this);

            this.BuildSql(statement);

            if (statement.Sorter != null)
            {
                statement.Sorter.BuildOrder(this.Sorter, this.Session.Database.Mapping, statement);
            }

            var objects = new List<ObjectId>();
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

        //TODO: Refactor this
        protected override void LazyLoadFilter()
        {
        }

        internal override string BuildSql(AllorsExtentStatementSql statement)
        {
            this.first.BuildSql(statement);

            switch (this.operationType)
            {
                case AllorsExtentOperationTypeSqlBundled.UNION:
                    statement.Append("\nUNION\n");
                    break;
                case AllorsExtentOperationTypeSqlBundled.INTERSECT:
                    statement.Append("\nINTERSECT\n");
                    break;
                case AllorsExtentOperationTypeSqlBundled.EXCEPT:
                    statement.Append("\nEXCEPT\n");
                    break;
            }

            this.second.BuildSql(statement);

            return null;
        }
    }
}