// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentStatementRoot.cs" company="Allors bvba">
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

        public override string ToString()
        {
            return this.sql.ToString();
        }

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

        internal override void Append(string part)
        {
            this.sql.Append(part);
        }

        internal override string CreateAlias()
        {
            return "alias" + (this.aliasIndex++);
        }

        internal override ExtentStatement CreateChild(SqlExtent extent, IAssociationType association)
        {
            return new ExtentStatementChild(this, extent, association);
        }

        internal override ExtentStatement CreateChild(SqlExtent extent, IRoleType role)
        {
            return new ExtentStatementChild(this, extent, role);
        }

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