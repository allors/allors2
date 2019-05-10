// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentOperation.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections.Generic;

    using Meta;

    internal class ExtentOperation : SqlExtent
    {
        private readonly SqlExtent first;
        private readonly ExtentOperations operationType;
        private readonly SqlExtent second;

        internal ExtentOperation(SqlExtent first, SqlExtent second, ExtentOperations operationType)
        {
            if (!first.ObjectType.Equals(second.ObjectType))
            {
                throw new ArgumentException("Both extents in a Union, Intersect or Except must be from the same type");
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

        internal override string BuildSql(ExtentStatement statement)
        {
            this.first.BuildSql(statement);

            switch (this.operationType)
            {
                case ExtentOperations.Union:
                    statement.Append("\nUNION\n");
                    break;
                case ExtentOperations.Intersect:
                    statement.Append("\nINTERSECT\n");
                    break;
                case ExtentOperations.Except:
                    statement.Append("\n" + this.Session.Database.Except + "\n");
                    break;
            }

            statement.Append("(");
            this.second.BuildSql(statement);
            statement.Append(")");

            return null;
        }

        protected override IList<long> GetObjectIds()
        {
            this.Session.Flush();

            var statement = new ExtentStatementRoot(this);
            var alias = this.BuildSql(statement);

            var objectIds = new List<long>();
            using (var command = statement.CreateDbCommand(alias))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var objectId = this.Session.State.GetObjectIdForExistingObject(reader.GetValue(0).ToString());
                        objectIds.Add(objectId);
                    }
                }
            }

            return objectIds;
        }

        // TODO: Refactor this
        protected override void LazyLoadFilter()
        {
        }
    }
}