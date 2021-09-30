// <copyright file="ExtentStatementRoot.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System.Collections.Generic;
    using System.Text;

    using Meta;

    internal class ExtentStatementRoot : ExtentStatement
    {
        private readonly Dictionary<object, string> paramNameByParamValue;
        private readonly StringBuilder sql;
        private int aliasIndex;
        private ICommand command;
        private int parameterIndex;

        internal ExtentStatementRoot(SqlExtent extent) : base(extent)
        {
            this.parameterIndex = 0;
            this.aliasIndex = 0;
            this.sql = new StringBuilder();
            this.paramNameByParamValue = new Dictionary<object, string>();
        }

        internal override bool IsRoot => true;

        public override string ToString() => this.sql.ToString();

        internal override string AddParameter(object obj)
        {
            if (!this.paramNameByParamValue.ContainsKey(obj))
            {
                var param = string.Format(this.Extent.Transaction.Database.Mapping.ParamInvocationFormat, "p" + this.parameterIndex++);
                this.paramNameByParamValue[obj] = param;
                return param;
            }

            return this.paramNameByParamValue[obj];
        }

        internal override void Append(string part) => this.sql.Append(part);

        internal override string CreateAlias() => "alias" + this.aliasIndex++;

        internal override ExtentStatement CreateChild(SqlExtent extent, IAssociationType association) => new ExtentStatementChild(this, extent, association);

        internal override ExtentStatement CreateChild(SqlExtent extent, IRoleType role) => new ExtentStatementChild(this, extent, role);

        internal ICommand CreateDbCommand(string alias)
        {
            if (this.sql.Length == 0)
            {
                return null;
            }

            if (this.Sorter != null)
            {
                if (alias == null)
                {
                    this.Sorter.BuildOrder(this);
                }
                else
                {
                    this.Sorter.BuildOrder(this, alias);
                }
            }

            this.command = this.Transaction.Connection.CreateCommand();
            this.command.CommandText = this.sql.ToString();

            foreach (var paramNameByParamValuePair in this.paramNameByParamValue)
            {
                var paramName = paramNameByParamValuePair.Value;
                var paramValue = paramNameByParamValuePair.Key;

                if (paramValue is IObject)
                {
                    this.command.AddInParameter(paramName, ((IObject)paramValue).Strategy.ObjectId);
                }
                else
                {
                    this.command.AddInParameter(paramName, paramValue);
                }
            }

            return this.command;
        }
    }
}
