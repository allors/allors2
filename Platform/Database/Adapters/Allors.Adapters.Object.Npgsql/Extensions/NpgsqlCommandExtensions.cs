// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NpgsqlCommandExtensions.cs" company="Allors bvba">
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
    using Allors.Adapters.Database.Sql;
    using global::Npgsql;
    using NpgsqlTypes;

    public static class NpgsqlCommandExtensions
    {
        public static void AddInObject(NpgsqlCommand command, SchemaParameter parameter, object value)
        {
            var sqlParameter = command.CreateParameter();
            sqlParameter.NpgsqlDbType = parameter.NpgsqlDbType;
            sqlParameter.ParameterName = parameter.Name;
            sqlParameter.Value = value ?? DBNull.Value;

            command.Parameters.Add(sqlParameter);
        }

        public static void SetInObject(NpgsqlCommand command, SchemaParameter param, object value)
        {
            command.Parameters[param.Name].Value = value ?? DBNull.Value;
        }

        public static void AddInTable(NpgsqlCommand command, SchemaArrayParameter parameter, object[] array)
        {
            var sqlParameter = command.CreateParameter();
            sqlParameter.NpgsqlDbType = NpgsqlDbType.Array | parameter.ElementType;
            sqlParameter.ParameterName = parameter.Name;
            sqlParameter.Value = array;

            command.Parameters.Add(sqlParameter);
        }

        public static void SetInTable(NpgsqlCommand command, SchemaArrayParameter param, object[] array)
        {
            command.Parameters[param.Name].Value = array;
        }
    }
}