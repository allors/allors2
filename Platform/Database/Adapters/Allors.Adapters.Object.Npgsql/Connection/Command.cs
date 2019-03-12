// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Command.cs" company="Allors bvba">
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
    using System.Data;
    using System.Linq;

    using global::Npgsql;

    using Allors.Meta;

    using NpgsqlTypes;

    public abstract class Command : IDisposable
    {
        protected Mapping Mapping { get; }

        protected NpgsqlCommand NpgsqlCommand { get; }

        protected internal Command(Mapping mapping, NpgsqlCommand command)
        {
            this.Mapping = mapping;
            this.NpgsqlCommand = command;
        }

        internal NpgsqlParameterCollection Parameters => this.NpgsqlCommand.Parameters;

        internal CommandType CommandType
        {
            get
            {
                return this.NpgsqlCommand.CommandType;
            }

            set
            {
                this.NpgsqlCommand.CommandType = value;
            }
        }

        internal string CommandText
        {
            get
            {
                return this.NpgsqlCommand.CommandText;
            }

            set
            {
                this.NpgsqlCommand.CommandText = value;
            }
        }

        public void Dispose()
        {
            this.NpgsqlCommand.Dispose();
        }

        internal NpgsqlParameter CreateParameter()
        {
            return this.NpgsqlCommand.CreateParameter();
        }

        internal void AddInParameter(string parameterName, object value)
        {
            var sqlParameter = this.NpgsqlCommand.Parameters.Contains(parameterName) ? this.NpgsqlCommand.Parameters[parameterName] : null;
            if (sqlParameter == null)
            {
                sqlParameter = this.NpgsqlCommand.CreateParameter();
                sqlParameter.ParameterName = parameterName;
                if (value is DateTime)
                {
                    sqlParameter.NpgsqlDbType = NpgsqlDbType.Timestamp;
                }

                this.NpgsqlCommand.Parameters.Add(sqlParameter);
            }

            if (value == null || value == DBNull.Value)
            {
                this.NpgsqlCommand.Parameters[parameterName].Value = DBNull.Value;
            }
            else
            {
                this.NpgsqlCommand.Parameters[parameterName].Value = value;
            }
        }

        internal void AddObjectParameter(long objectId)
        {
            var sqlParameter = this.NpgsqlCommand.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForObject;
            sqlParameter.NpgsqlDbType = Mapping.NpgsqlDbTypeForObject;
            sqlParameter.Value = objectId;

            this.NpgsqlCommand.Parameters.Add(sqlParameter);
        }

        internal void AddTypeParameter(IClass @class)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForClass;
            sqlParameter.NpgsqlDbType = Mapping.NpgsqlDbTypeForClass;
            sqlParameter.Value = @class.Id;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddCountParameter(int count)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForCount;
            sqlParameter.NpgsqlDbType = Mapping.NpgsqlDbTypeForCount;
            sqlParameter.Value = count;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddCompositeRoleParameter(long objectId)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForCompositeRole;
            sqlParameter.NpgsqlDbType = Mapping.NpgsqlDbTypeForObject;
            sqlParameter.Value = objectId;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddAssociationParameter(long objectId)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForAssociation;
            sqlParameter.NpgsqlDbType = Mapping.NpgsqlDbTypeForObject;
            sqlParameter.Value = objectId;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddObjectTableParameter(IEnumerable<Reference> references)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.NpgsqlDbType = NpgsqlDbType.Array | Mapping.NpgsqlDbTypeForObject;
            sqlParameter.ParameterName = Mapping.ParamNameForTableType;
            sqlParameter.Value = references.Select(v => v.ObjectId);

            this.Parameters.Add(sqlParameter);
        }

        internal void AddObjectTableParameter(IEnumerable<long> objectIds)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.NpgsqlDbType = NpgsqlDbType.Array | Mapping.NpgsqlDbTypeForObject;
            sqlParameter.ParameterName = Mapping.ParamNameForTableType;
            sqlParameter.Value = objectIds;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddCompositeRoleTableParameter(IEnumerable<CompositeRelation> relations)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.NpgsqlDbType = NpgsqlDbType.Array | Mapping.NpgsqlDbTypeForObject;
            sqlParameter.ParameterName = Mapping.ParamNameForTableType;
            sqlParameter.Value = relations.Select(v => v.Role);

            this.Parameters.Add(sqlParameter);
        }

        internal void AddAssociationTableParameter(long objectId)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForAssociation;
            sqlParameter.NpgsqlDbType = Mapping.NpgsqlDbTypeForObject;
            sqlParameter.Value = objectId;

            this.Parameters.Add(sqlParameter);
        }

        internal object ExecuteScalar()
        {
            this.OnExecuting();
            try
            {
                return this.NpgsqlCommand.ExecuteScalar();
            }
            finally
            {
                this.OnExecuted();
            }
        }

        internal void ExecuteNonQuery()
        {
            this.OnExecuting();

            try
            {
                this.NpgsqlCommand.ExecuteNonQuery();
            }
            finally
            {
                this.OnExecuted();
            }
        }

        internal NpgsqlDataReader ExecuteReader()
        {
            this.OnExecuting();

            try
            {
                return this.NpgsqlCommand.ExecuteReader();
            }
            finally
            {
                this.OnExecuted();
            }

        }

        internal object GetValue(NpgsqlDataReader reader, UnitTags unitTypeTag, int i)
        {
            switch (unitTypeTag)
            {
                case UnitTags.String:
                    return reader.GetString(i);
                case UnitTags.Integer:
                    return reader.GetInt32(i);
                case UnitTags.Float:
                    return reader.GetDouble(i);
                case UnitTags.Decimal:
                    return reader.GetDecimal(i);
                case UnitTags.Boolean:
                    return reader.GetBoolean(i);
                case UnitTags.DateTime:
                    return reader.GetDateTime(i);
                case UnitTags.Unique:
                    return reader.GetGuid(i);
                case UnitTags.Binary:
                    return reader.GetValue(i);
                default:
                    throw new ArgumentException("Unknown Unit ObjectType: " + unitTypeTag);
            }
        }

        #region Events

        protected abstract void OnExecuting();

        protected abstract void OnExecuted();

        #endregion
    }
}