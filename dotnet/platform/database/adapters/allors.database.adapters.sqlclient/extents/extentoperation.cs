// <copyright file="ExtentOperation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;

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

        public override ICompositePredicate Filter => null;

        internal override Session Session => this.first.Session;

        public override IComposite ObjectType => this.first.ObjectType;

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
