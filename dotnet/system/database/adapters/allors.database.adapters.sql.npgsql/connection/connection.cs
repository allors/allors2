// <copyright file="Connection.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using global::Npgsql;

    public class Connection : IConnection
    {
        internal readonly Database Database;

        internal Connection(Database database) => this.Database = database;

        internal NpgsqlConnection NpgsqlConnection { get; private set; }

        internal NpgsqlTransaction NpgsqlTransaction { get; private set; }

        public ICommand CreateCommand()
        {
            if (this.NpgsqlConnection == null)
            {
                this.NpgsqlConnection = new NpgsqlConnection(this.Database.ConnectionString);
                this.NpgsqlConnection.Open();
                this.NpgsqlTransaction = this.NpgsqlConnection.BeginTransaction(this.Database.IsolationLevel ?? Database.DefaultIsolationLevel);
            }

            var sqlCommand = this.NpgsqlConnection.CreateCommand();
            sqlCommand.Transaction = this.NpgsqlTransaction;
            if (this.Database.CommandTimeout.HasValue)
            {
                sqlCommand.CommandTimeout = this.Database.CommandTimeout.Value;
            }

            return this.CreateCommand((Mapping)this.Database.Mapping, sqlCommand);
        }

        public void Commit()
        {
            try
            {
                if (this.NpgsqlTransaction != null)
                {
                    this.NpgsqlTransaction.Commit();
                }
            }
            finally
            {
                this.NpgsqlTransaction = null;

                if (this.NpgsqlConnection != null)
                {
                    this.NpgsqlConnection?.Close();
                }

                this.NpgsqlConnection = null;
            }
        }

        public void Rollback()
        {
            try
            {
                if (this.NpgsqlTransaction != null)
                {
                    this.NpgsqlTransaction?.Rollback();
                }
            }
            finally
            {
                this.NpgsqlTransaction = null;

                if (this.NpgsqlConnection != null)
                {
                    this.NpgsqlConnection?.Close();
                }

                this.NpgsqlConnection = null;
            }
        }

        private Command CreateCommand(Mapping mapping, NpgsqlCommand sqlCommand) => new Command(mapping, sqlCommand);
    }
}
