// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaParameter.cs" company="Allors bvba">
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

namespace Allors.Adapters.Database.Npgsql
{
    using System;
    using System.Data;

    using NpgsqlTypes;

    public class SchemaParameter : Sql.SchemaParameter
    {
        public SchemaParameter(Sql.Schema schema, string name, DbType type)
            : base(schema, name, type)
        {
        }

        public NpgsqlDbType NpgsqlDbType
        {
            get
            {
                switch (this.DbType)
                {
                    case DbType.String:
                        return NpgsqlDbType.Varchar;
                    case DbType.Int32:
                        return NpgsqlDbType.Integer;
                    case DbType.Int64:
                        return NpgsqlDbType.Bigint;
                    case DbType.Decimal:
                        return NpgsqlDbType.Numeric;
                    case DbType.Double:
                        return NpgsqlDbType.Double;
                    case DbType.Boolean:
                        return NpgsqlDbType.Boolean;
                    case DbType.Date:
                        return NpgsqlDbType.Date;
                    case DbType.DateTime:
                        return NpgsqlDbType.Timestamp;
                    case DbType.Guid:
                        return NpgsqlDbType.Uuid;
                    case DbType.Binary:
                        return NpgsqlDbType.Bytea;
                    default:
                        throw new Exception("!UNKNOWN VALUE TYPE!");
                }
            }
        }
    }
}