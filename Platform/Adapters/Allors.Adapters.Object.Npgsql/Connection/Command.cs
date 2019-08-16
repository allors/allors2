
// <copyright file="Command.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
        protected internal Command(Mapping mapping, NpgsqlCommand command)
        {
            this.Mapping = mapping;
            this.NpgsqlCommand = command;
        }

        internal NpgsqlParameterCollection Parameters => this.NpgsqlCommand.Parameters;

        internal CommandType CommandType
        {
            get => this.NpgsqlCommand.CommandType;
            set => this.NpgsqlCommand.CommandType = value;
        }

        internal string CommandText
        {
            get => this.NpgsqlCommand.CommandText;
            set => this.NpgsqlCommand.CommandText = value;
        }

        protected Mapping Mapping { get; }

        protected NpgsqlCommand NpgsqlCommand { get; }

        public void Dispose() => this.NpgsqlCommand.Dispose();

        internal NpgsqlParameter CreateParameter() => this.NpgsqlCommand.CreateParameter();

        internal NpgsqlParameter GetParameter(string name) => this.NpgsqlCommand.Parameters[name];

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

        internal void AddObjectArrayParameter(IEnumerable<Reference> references)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.NpgsqlDbType = NpgsqlDbType.Array | Mapping.NpgsqlDbTypeForObject;
            sqlParameter.ParameterName = this.Mapping.ObjectArrayParam.InvocationName;
            sqlParameter.Value = references.Select(v => v.ObjectId).ToArray();

            this.Parameters.Add(sqlParameter);
        }

        internal void SetObjectArrayParameter(IEnumerable<Reference> references)
        {
            var objectParameter = this.GetParameter(this.Mapping.ObjectArrayParam.InvocationName);
            objectParameter.Value = references.Select(v => v.ObjectId).ToArray();
        }

        internal void AddObjectArrayParameter(IEnumerable<long> objectIds)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.NpgsqlDbType = NpgsqlDbType.Array | Mapping.NpgsqlDbTypeForObject;
            sqlParameter.ParameterName = this.Mapping.ObjectArrayParam.InvocationName;
            sqlParameter.Value = objectIds.ToArray();

            this.Parameters.Add(sqlParameter);
        }

        internal void SetObjectArrayParameter(IEnumerable<long> objectIds)
        {
            var objectParameter = this.GetParameter(this.Mapping.ObjectArrayParam.InvocationName);
            objectParameter.Value = objectIds.ToArray();
        }

        internal void AddUnitRoleArrayParameter(IRoleType roleType, ICollection<UnitRelation> relations)
        {
            var objectParameter = this.CreateParameter();
            objectParameter.NpgsqlDbType = NpgsqlDbType.Array | Mapping.NpgsqlDbTypeForObject;
            objectParameter.ParameterName = this.Mapping.ObjectArrayParam.InvocationName;

            var roleParameter = this.CreateParameter();
            roleParameter.NpgsqlDbType = NpgsqlDbType.Array | this.Mapping.GetNpgsqlDbType(roleType);
            roleParameter.ParameterName = this.Mapping.StringRoleArrayParam.InvocationName; // TODO: should be a shared name

            objectParameter.Value = relations.Select(v => v.Association).ToArray();
            roleParameter.Value = relations.Select(v => v.Role).ToArray();

            this.Parameters.Add(objectParameter);
            this.Parameters.Add(roleParameter);
        }

        internal void SetUnitRoleArrayParameter(IRoleType roleType, ICollection<UnitRelation> relations)
        {
            var objectParameter = this.GetParameter(this.Mapping.ObjectArrayParam.InvocationName);
            var roleParameter = this.GetParameter(this.Mapping.StringRoleArrayParam.InvocationName); // TODO: should be a shared name

            objectParameter.Value = relations.Select(v => v.Association).ToArray();
            roleParameter.Value = relations.Select(v => v.Role).ToArray();
        }

        internal void AddCompositeRoleArrayParameter(ICollection<CompositeRelation> relations)
        {
            var objectParameter = this.CreateParameter();
            objectParameter.NpgsqlDbType = NpgsqlDbType.Array | Mapping.NpgsqlDbTypeForObject;
            objectParameter.ParameterName = this.Mapping.ObjectArrayParam.InvocationName;

            var roleParameter = this.CreateParameter();
            roleParameter.NpgsqlDbType = NpgsqlDbType.Array | Mapping.NpgsqlDbTypeForObject;
            roleParameter.ParameterName = this.Mapping.StringRoleArrayParam.InvocationName; // TODO: should be a shared name

            objectParameter.Value = relations.Select(v => v.Association).ToArray();
            roleParameter.Value = relations.Select(v => v.Role).ToArray();

            this.Parameters.Add(objectParameter);
            this.Parameters.Add(roleParameter);
        }

        internal void SetCompositeRoleArrayParameter(ICollection<CompositeRelation> relations)
        {
            var objectParameter = this.GetParameter(this.Mapping.ObjectArrayParam.InvocationName);
            var roleParameter = this.GetParameter(this.Mapping.StringRoleArrayParam.InvocationName); // TODO: should be a shared name

            objectParameter.Value = relations.Select(v => v.Association).ToArray();
            roleParameter.Value = relations.Select(v => v.Role).ToArray();
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
