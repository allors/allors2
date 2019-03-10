// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Command.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.Adapters.Database.Sql
{
    using System;
    using System.Data.Common;

    using Allors.Meta;

    public abstract class Command : IDisposable
    {
        protected abstract DbCommand DbCommand { get; }

        public abstract void Dispose();

        public abstract void AddInParameter(string parameter, object value);

        public abstract void AddInParameter(SchemaParameter parameter, object value);

        public abstract void AddInParameter(DbParameter parameter);

        public abstract void AddOutParameter(SchemaParameter parameter);

        public virtual byte[] GetBinary(DbDataReader reader, int i)
        {
            return (byte[])reader.GetValue(i);
        }

        public virtual bool GetBoolean(DbDataReader reader, int i)
        {
            return reader.GetBoolean(i);
        }

        public virtual DateTime GetDateTime(DbDataReader reader, int i)
        {
            return reader.GetDateTime(i);
        }

        public virtual decimal GetDecimal(DbDataReader reader, int i)
        {
            return reader.GetDecimal(i);
        }

        public virtual double GetDouble(DbDataReader reader, int i)
        {
            return reader.GetDouble(i);
        }

        public virtual int GetInteger(DbDataReader reader, int i)
        {
            return reader.GetInt32(i);
        }

        public virtual long GetLong(DbDataReader reader, int i)
        {
            return reader.GetInt64(i);
        }

        public virtual object GetParameterValue(SchemaParameter parameter)
        {
            return this.DbCommand.Parameters[parameter.Name].Value;
        }

        public virtual string GetString(DbDataReader reader, int i)
        {
            return reader.GetString(i);
        }

        public virtual Guid GetUnique(DbDataReader reader, int i)
        {
            return reader.GetGuid(i);
        }

        public virtual object GetValue(DbDataReader reader, int i)
        {
            return reader.GetValue(i);
        }

        public virtual object GetValue(DbDataReader reader, UnitTags unitTypeTag, int i)
        {
            switch (unitTypeTag)
            {
                case UnitTags.String:
                    return this.GetString(reader, i);
                case UnitTags.Integer:
                    return this.GetInteger(reader, i);
                case UnitTags.Decimal:
                    return this.GetDecimal(reader, i);
                case UnitTags.Float:
                    return this.GetDouble(reader, i);
                case UnitTags.Boolean:
                    return this.GetBoolean(reader, i);
                case UnitTags.DateTime:
                    return this.GetDateTime(reader, i);
                case UnitTags.Unique:
                    return this.GetUnique(reader, i);
                case UnitTags.Binary:
                    return this.GetBinary(reader, i);
                default:
                    throw new ArgumentException("Unknown Unit IObjectType: " + unitTypeTag);
            }
        }

        public abstract void SetParameterValue(SchemaParameter parameter, object value);

        public virtual void ExecuteNonQuery()
        {
            this.DbCommand.ExecuteNonQuery();
        }

        public virtual DbDataReader ExecuteReader()
        {
            return this.DbCommand.ExecuteReader();
        }

        public virtual object ExecuteReaderGetValue()
        {
            using (var reader = this.ExecuteReader())
            {
                if (reader.Read())
                {
                    return this.GetValue(reader, 0);
                }

                throw new Exception("Reader returned no rows");
            }
        }
    }
}