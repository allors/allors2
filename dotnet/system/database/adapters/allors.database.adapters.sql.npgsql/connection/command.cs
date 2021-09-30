// <copyright file="Command.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Meta;
    using global::Npgsql;
    using NpgsqlTypes;

    public class Command : ICommand
    {
        private readonly Mapping mapping;

        private readonly NpgsqlCommand command;

        public Command(Mapping mapping, NpgsqlCommand command)
        {
            this.mapping = mapping;
            this.command = command;
        }

        public CommandType CommandType
        {
            get => this.command.CommandType;
            set => this.command.CommandType = value;
        }

        public string CommandText
        {
            get => this.command.CommandText;
            set => this.command.CommandText = value;
        }

        public void Dispose() => this.command.Dispose();

        public void AddInParameter(string parameterName, object value)
        {
            var sqlParameter = this.command.Parameters.Contains(parameterName) ? this.command.Parameters[parameterName] : null;
            if (sqlParameter == null)
            {
                sqlParameter = this.command.CreateParameter();
                sqlParameter.ParameterName = parameterName;
                if (value is DateTime)
                {
                    sqlParameter.NpgsqlDbType = NpgsqlDbType.Timestamp;
                }

                this.command.Parameters.Add(sqlParameter);
            }

            if (value == null || value == DBNull.Value)
            {
                this.command.Parameters[parameterName].Value = DBNull.Value;
            }
            else
            {
                this.command.Parameters[parameterName].Value = value;
            }
        }

        public void ObjectParameter(long objectId) => this.GetOrCreateParameter(this.mapping.ParamInvocationNameForObject, Mapping.NpgsqlDbTypeForObject).Value = objectId;

        public void AddTypeParameter(IClass @class) => this.GetOrCreateParameter(this.mapping.ParamInvocationNameForClass, Mapping.NpgsqlDbTypeForClass).Value = @class.Id;

        public void AddCountParameter(int count) => this.GetOrCreateParameter(this.mapping.ParamInvocationNameForCount, Mapping.NpgsqlDbTypeForCount).Value = count;

        public void AddUnitRoleParameter(IRoleType roleType, object unit) => this.GetOrCreateParameter(this.mapping.ParamInvocationNameByRoleType[roleType], this.mapping.GetNpgsqlDbType(roleType)).Value = unit ?? DBNull.Value;

        public void AddCompositeRoleParameter(long objectId) => this.GetOrCreateParameter(this.mapping.ParamInvocationNameForCompositeRole, Mapping.NpgsqlDbTypeForObject).Value = objectId;

        public void AddAssociationParameter(long objectId) => this.GetOrCreateParameter(this.mapping.ParamInvocationNameForAssociation, Mapping.NpgsqlDbTypeForObject).Value = objectId;

        public void ObjectTableParameter(IEnumerable<long> objectIds) => this.GetOrCreateTableParameter(this.mapping.ObjectArrayParam.InvocationName, Mapping.NpgsqlDbTypeForObject).Value = objectIds.ToArray();

        public void UnitTableParameter(IRoleType roleType, IEnumerable<UnitRelation> relations)
        {
            var objectParameter = this.GetOrCreateTableParameter(this.mapping.ObjectArrayParam.InvocationName, Mapping.NpgsqlDbTypeForObject);
            var roleParameter = this.GetOrCreateTableParameter(this.mapping.StringRoleArrayParam.InvocationName, this.mapping.GetNpgsqlDbType(roleType));

            var unitRelations = relations as ICollection<UnitRelation> ?? relations.ToArray();
            objectParameter.Value = unitRelations.Select(v => v.Association).ToArray();
            roleParameter.Value = unitRelations.Select(v => v.Role).ToArray();
        }

        public void AddCompositeRoleTableParameter(IEnumerable<CompositeRelation> relations)
        {
            var objectParameter = this.GetOrCreateTableParameter(this.mapping.ObjectArrayParam.InvocationName, Mapping.NpgsqlDbTypeForObject);
            var roleParameter = this.GetOrCreateTableParameter(this.mapping.StringRoleArrayParam.InvocationName, Mapping.NpgsqlDbTypeForObject);

            var compositeRelations = relations as ICollection<CompositeRelation> ?? relations.ToArray();
            objectParameter.Value = compositeRelations.Select(v => v.Association).ToArray();
            roleParameter.Value = compositeRelations.Select(v => v.Role).ToArray();
        }

        public object ExecuteScalar() => this.command.ExecuteScalar();

        public void ExecuteNonQuery() => this.command.ExecuteNonQuery();

        public object GetValue(IReader reader, string tag, int i) =>
            tag switch
            {
                UnitTags.String => reader.GetString(i),
                UnitTags.Integer => reader.GetInt32(i),
                UnitTags.Float => reader.GetDouble(i),
                UnitTags.Decimal => reader.GetDecimal(i),
                UnitTags.Boolean => reader.GetBoolean(i),
                UnitTags.DateTime => reader.GetDateTime(i),
                UnitTags.Unique => reader.GetGuid(i),
                UnitTags.Binary => reader.GetValue(i),
                _ => throw new ArgumentException("Unknown Unit Tag: " + tag),
            };

        public IReader ExecuteReader() => new Reader(this.command.ExecuteReader());

        private NpgsqlParameter GetOrCreateParameter(string parameterName, NpgsqlDbType dbType)
        {
            var parameter = this.command.Parameters.Contains(parameterName) ? this.command.Parameters[parameterName] : null;
            if (parameter != null)
            {
                return parameter;
            }

            parameter = this.command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.NpgsqlDbType = dbType;
            this.command.Parameters.Add(parameter);

            return parameter;
        }

        private NpgsqlParameter GetOrCreateTableParameter(string parameterName, NpgsqlDbType sqlDbType)
        {
            var parameter = this.command.Parameters.Contains(parameterName) ? this.command.Parameters[parameterName] : null;
            if (parameter != null)
            {
                return parameter;
            }

            parameter = this.command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.NpgsqlDbType = NpgsqlDbType.Array | sqlDbType;
            this.command.Parameters.Add(parameter);

            return parameter;
        }

        // TODO: Review
        public void AddCompositesRoleTableParameter(IEnumerable<long> objectIds) => this.ObjectTableParameter(objectIds);
    }
}
