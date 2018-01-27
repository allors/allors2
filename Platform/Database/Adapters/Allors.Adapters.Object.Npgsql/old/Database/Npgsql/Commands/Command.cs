// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Command.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.Database.Npgsql.Commands
{
    using System;
    using System.Collections.Generic;

    using global::Npgsql;

    using NpgsqlTypes;

    using SchemaParameter = SchemaParameter;

    public abstract class Command
    {
        public void AddInObject(NpgsqlCommand command, Sql.SchemaParameter parameter, object value)
        {
            var schemaParameter = (SchemaParameter)parameter;

            var sqlParameter = command.CreateParameter();
            sqlParameter.NpgsqlDbType = schemaParameter.NpgsqlDbType;
            sqlParameter.ParameterName = parameter.Name;
            sqlParameter.Value = Normalize(value);
            
            command.Parameters.Add(sqlParameter);
        }

        public void SetInObject(NpgsqlCommand command, Sql.SchemaParameter param, object value)
        {
            command.Parameters[param.Name].Value = Normalize(value);
        }

        public void AddInTable(NpgsqlCommand command, SchemaArrayParameter parameter, object[] array)
        {
            var sqlParameter = command.CreateParameter();
            sqlParameter.NpgsqlDbType = NpgsqlDbType.Array | parameter.ElementType;
            sqlParameter.ParameterName = parameter.Name;
            sqlParameter.Value = array;

            command.Parameters.Add(sqlParameter);
        }

        public void SetInTable(NpgsqlCommand command, SchemaArrayParameter param, object[] array)
        {
            command.Parameters[param.Name].Value = array;
        }

        public int GetCachId(NpgsqlDataReader reader, int i)
        {
            return reader.GetInt32(i);
        }

        public Guid GetClassId(NpgsqlDataReader reader, int i)
        {
            return reader.GetGuid(i);
        }

        private static object Normalize(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value;
        }
    }
}