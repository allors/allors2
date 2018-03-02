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

namespace Allors.Adapters.Database.Npgsql
{
    using System;
    using System.Data;
    using System.Data.Common;

    using global::Npgsql;

    using NpgsqlTypes;

    internal class Command : Sql.Command
    {
        private readonly NpgsqlCommand command;

        public Command(ICommandFactory commandFactory, string commandText)
        {
            this.command = commandFactory.CreateNpgsqlCommand(commandText);
        }

        protected override DbCommand DbCommand
        {
            get { return this.command; }
        }

        public override void AddInParameter(string parameterName, object value)
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

            this.SetParameterValue(parameterName, value);
        }

        public override void AddInParameter(Sql.SchemaParameter parameter, object value)
        {
            var sqlParameter = this.command.Parameters.Contains(parameter.Name) ? this.command.Parameters[parameter.Name] : null;
            if (sqlParameter == null)
            {
                sqlParameter = this.command.CreateParameter();
                sqlParameter.DbType = parameter.DbType;
                sqlParameter.ParameterName = parameter.Name;
                this.command.Parameters.Add(sqlParameter);
            }

            this.SetParameterValue(parameter.Name, value);
        }

        public override void AddInParameter(DbParameter parameter)
        {
            if (this.command.Parameters.Contains(parameter.ParameterName))
            {
                this.command.Parameters.Remove(this.command.Parameters[parameter.ParameterName]);
            }

            this.command.Parameters.Add(parameter);
        }

        public override void AddOutParameter(Sql.SchemaParameter parameter)
        {
            var sqlParameter = this.command.Parameters.Contains(parameter.Name) ? this.command.Parameters[parameter.Name] : null;
            if (sqlParameter == null)
            {
                sqlParameter = this.command.CreateParameter();
                sqlParameter.ParameterName = parameter.Name;
                sqlParameter.DbType = parameter.DbType;
                sqlParameter.Direction = ParameterDirection.Output;
                this.command.Parameters.Add(sqlParameter);
            }

            this.command.Parameters.Add(sqlParameter);
        }

        public override void SetParameterValue(Sql.SchemaParameter parameter, object value)
        {
            this.SetParameterValue(parameter.Name, value);
        }

        public override void Dispose()
        {
            this.command.Dispose();
        }

        private void SetParameterValue(string parameterName, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                this.command.Parameters[parameterName].Value = DBNull.Value;
            }
            else
            {
                this.command.Parameters[parameterName].Value = value;
            }
        }
    }
}