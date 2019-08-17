// <copyright file="ExtentStatementRoot.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
{
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using Allors.Meta;

    internal class ExtentStatementRoot : ExtentStatement
    {
        private readonly Dictionary<object, string> paramInvocationNameByParamValue;
        private readonly Dictionary<string, string> paramNameByParamInvocationName;
        private readonly StringBuilder sql;
        private int aliasIndex;
        private Command command;
        private int parameterIndex;

        internal ExtentStatementRoot(SqlExtent extent) : base(extent)
        {
            this.parameterIndex = 0;
            this.aliasIndex = 0;
            this.sql = new StringBuilder();
            this.paramInvocationNameByParamValue = new Dictionary<object, string>();
            this.paramNameByParamInvocationName = new Dictionary<string, string>();
        }

        internal override bool IsRoot => true;

        public override string ToString() => this.sql.ToString();

        internal override string AddParameter(object obj)
        {
            if (!this.paramInvocationNameByParamValue.ContainsKey(obj))
            {
                var name = $"p{this.parameterIndex++}";

                var paramInvocationName = string.Format(Mapping.ParamInvocationFormat, name);
                this.paramInvocationNameByParamValue[obj] = paramInvocationName;

                var paramName = string.Format(Mapping.ParamFormat, name);
                this.paramNameByParamInvocationName[paramInvocationName] = paramName;

                return paramInvocationName;
            }

            return this.paramInvocationNameByParamValue[obj];
        }

        internal override void Append(string part) => this.sql.Append(part);

        internal override string CreateAlias() => "alias" + this.aliasIndex++;

        internal override ExtentStatement CreateChild(SqlExtent extent, IAssociationType association) => new ExtentStatementChild(this, extent, association);

        internal override ExtentStatement CreateChild(SqlExtent extent, IRoleType role) => new ExtentStatementChild(this, extent, role);

        internal Command CreateDbCommand(string alias)
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

            this.command = this.Session.Connection.CreateCommand();
            this.command.CommandText = this.sql.ToString();
            this.command.CommandType = CommandType.Text;

            foreach (var paramInvocationNameByParamValuePair in this.paramInvocationNameByParamValue)
            {
                var paramInvocationName = paramInvocationNameByParamValuePair.Value;
                var paramValue = paramInvocationNameByParamValuePair.Key;

                var paramName = this.paramNameByParamInvocationName[paramInvocationName];

                this.command.AddInParameter(
                    paramName,
                    paramValue is IObject @object ? @object.Strategy.ObjectId : paramValue);
            }

            return this.command;
        }
    }
}
