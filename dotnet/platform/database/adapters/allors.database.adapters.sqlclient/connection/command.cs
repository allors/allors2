// <copyright file="Command.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using Allors.Meta;

    public abstract class Command : IDisposable
    {
        protected internal Command(Mapping mapping, SqlCommand command)
        {
            this.Mapping = mapping;
            this.SqlCommand = command;
        }

        internal SqlParameterCollection Parameters => this.SqlCommand.Parameters;

        internal CommandType CommandType
        {
            get => this.SqlCommand.CommandType;

            set => this.SqlCommand.CommandType = value;
        }

        internal string CommandText
        {
            get => this.SqlCommand.CommandText;

            set => this.SqlCommand.CommandText = value;
        }

        protected Mapping Mapping { get; }

        protected SqlCommand SqlCommand { get; }

        public void Dispose() => this.SqlCommand.Dispose();

        internal SqlParameter CreateParameter() => this.SqlCommand.CreateParameter();

        internal void AddInParameter(string parameterName, object value)
        {
            var sqlParameter = this.SqlCommand.Parameters.Contains(parameterName) ? this.SqlCommand.Parameters[parameterName] : null;
            if (sqlParameter == null)
            {
                sqlParameter = this.SqlCommand.CreateParameter();
                sqlParameter.ParameterName = parameterName;
                if (value is DateTime)
                {
                    sqlParameter.SqlDbType = SqlDbType.DateTime2;
                }

                this.SqlCommand.Parameters.Add(sqlParameter);
            }

            if (value == null || value == DBNull.Value)
            {
                this.SqlCommand.Parameters[parameterName].Value = DBNull.Value;
            }
            else
            {
                this.SqlCommand.Parameters[parameterName].Value = value;
            }
        }

        internal void AddObjectParameter(long objectId)
        {
            var sqlParameter = this.SqlCommand.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForObject;
            sqlParameter.SqlDbType = Mapping.SqlDbTypeForObject;
            sqlParameter.Value = objectId;

            this.SqlCommand.Parameters.Add(sqlParameter);
        }

        internal void AddTypeParameter(IClass @class)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForClass;
            sqlParameter.SqlDbType = Mapping.SqlDbTypeForClass;
            sqlParameter.Value = @class.Id;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddCountParameter(int count)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForCount;
            sqlParameter.SqlDbType = Mapping.SqlDbTypeForCount;
            sqlParameter.Value = count;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddCompositeRoleParameter(long objectId)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForCompositeRole;
            sqlParameter.SqlDbType = Mapping.SqlDbTypeForObject;
            sqlParameter.Value = objectId;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddAssociationParameter(long objectId)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForAssociation;
            sqlParameter.SqlDbType = Mapping.SqlDbTypeForObject;
            sqlParameter.Value = objectId;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddObjectTableParameter(IEnumerable<Reference> references)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.SqlDbType = SqlDbType.Structured;
            sqlParameter.TypeName = this.Mapping.TableTypeNameForObject;
            sqlParameter.ParameterName = Mapping.ParamNameForTableType;
            sqlParameter.Value = new CompositesRoleDataRecords(this.Mapping, references);

            this.Parameters.Add(sqlParameter);
        }

        internal void AddObjectTableParameter(IEnumerable<long> objectIds)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.SqlDbType = SqlDbType.Structured;
            sqlParameter.TypeName = this.Mapping.TableTypeNameForObject;
            sqlParameter.ParameterName = Mapping.ParamNameForTableType;
            sqlParameter.Value = new ObjectDataRecord(this.Mapping, objectIds);
            ;

            this.Parameters.Add(sqlParameter);
        }

        internal void AddCompositeRoleTableParameter(IEnumerable<CompositeRelation> relations)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.SqlDbType = SqlDbType.Structured;
            sqlParameter.TypeName = this.Mapping.TableTypeNameForCompositeRelation;
            sqlParameter.ParameterName = Mapping.ParamNameForTableType;
            sqlParameter.Value = new CompositeRoleDataRecords(this.Mapping, relations);

            this.Parameters.Add(sqlParameter);
        }

        internal void AddAssociationTableParameter(long objectId)
        {
            var sqlParameter = this.CreateParameter();
            sqlParameter.ParameterName = Mapping.ParamNameForAssociation;
            sqlParameter.SqlDbType = Mapping.SqlDbTypeForObject;
            sqlParameter.Value = objectId;

            this.Parameters.Add(sqlParameter);
        }

        internal object ExecuteScalar()
        {
            this.OnExecuting();
            try
            {
                return this.SqlCommand.ExecuteScalar();
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
                this.SqlCommand.ExecuteNonQuery();
            }
            finally
            {
                this.OnExecuted();
            }
        }

        internal SqlDataReader ExecuteReader()
        {
            this.OnExecuting();

            try
            {
                return this.SqlCommand.ExecuteReader();
            }
            finally
            {
                this.OnExecuted();
            }
        }

        internal object GetValue(SqlDataReader reader, UnitTags unitTypeTag, int i)
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

        #endregion Events
    }
}
