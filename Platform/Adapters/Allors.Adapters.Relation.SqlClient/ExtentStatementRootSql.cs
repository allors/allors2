//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsExtentStatementRootSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsExtentStatementSql type.</summary>
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Allors.Meta;

namespace Allors.Adapters.Relation.SqlClient
{
    internal class AllorsExtentStatementRootSql : AllorsExtentStatementSql
    {
        private readonly Dictionary<object, string> paramNameByParamValue;
        private readonly StringBuilder sql;
        private int aliasIndex;
        private int parameterIndex;

        internal AllorsExtentStatementRootSql(AllorsExtentSql extent)
            : base(extent)
        {
            this.parameterIndex = 0;
            this.aliasIndex = 0;
            this.sql = new StringBuilder();
            this.paramNameByParamValue = new Dictionary<object, string>();
        }

        internal override bool IsRoot => true;

        public override string ToString()
        {
            return this.sql.ToString();
        }

        internal override string AddParameter(object obj)
        {
            if (!this.paramNameByParamValue.ContainsKey(obj))
            {
                var param = Mapping.ParamPrefix + "p" + (this.parameterIndex++);
                this.paramNameByParamValue[obj] = param;
                return param;
            }

            return this.paramNameByParamValue[obj];
        }

        internal override void Append(string part)
        {
            this.sql.Append(part);
        }

        internal override string CreateAlias()
        {
            return "alias" + (this.aliasIndex++);
        }

        internal override AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IAssociationType association)
        {
            return new AllorsExtentStatementChildSql(this, extent, association);
        }

        internal override AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IRoleType roleType)
        {
            return new AllorsExtentStatementChildSql(this, extent, roleType);
        }

        internal SqlCommand CreateSqlCommand()
        {
            var command = Session.CreateCommand(this.sql.ToString());

            foreach (KeyValuePair<object, string> paramNameByParamValuePair in this.paramNameByParamValue)
            {
                var paramName = paramNameByParamValuePair.Value;
                var paramValue = paramNameByParamValuePair.Key;

                if (paramValue == null || paramValue == DBNull.Value)
                {
                    command.Parameters.AddWithValue(paramName, DBNull.Value);
                }
                else if (paramValue is IObject)
                {
                    command.Parameters.AddWithValue(paramName, ((IObject)paramValue).Id);
                }
                else
                {
                    var param = command.Parameters.AddWithValue(paramName, paramValue);

                    if (paramValue is DateTime)
                    {
                        param.SqlDbType = SqlDbType.DateTime2;
                    }
                }
            }

            return command;
        }
    }
}