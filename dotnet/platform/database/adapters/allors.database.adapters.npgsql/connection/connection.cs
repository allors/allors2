// <copyright file="Connection.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql
{
    using global::Npgsql;

    public abstract class Connection
    {
        protected readonly Database Database;

        protected Connection(Database database) => this.Database = database;

        protected NpgsqlConnection NpgsqlConnection { get; private set; }

        protected NpgsqlTransaction NpgsqlTransaction { get; private set; }

        internal Command CreateCommand()
        {
            if (this.NpgsqlConnection == null)
            {
                this.OnCreatingNpgsqlConnection();
                this.NpgsqlConnection = new NpgsqlConnection(this.Database.ConnectionString);
                this.OnCreatedNpgsqlConnection();

                this.OnOpeningNpgsqlConnection();
                this.NpgsqlConnection.Open();
                this.OnOpenedNpgsqlConnection();

                this.OnCreatingNpgsqlTransaction();
                this.NpgsqlTransaction = this.NpgsqlConnection.BeginTransaction(this.Database.IsolationLevel ?? Database.DefaultIsolationLevel);
                this.OnCreatedNpgsqlTransaction();
            }

            this.OnCreatingNpgsqlCommand();
            var sqlCommand = this.NpgsqlConnection.CreateCommand();
            sqlCommand.Transaction = this.NpgsqlTransaction;
            if (this.Database.CommandTimeout.HasValue)
            {
                sqlCommand.CommandTimeout = this.Database.CommandTimeout.Value;
            }

            this.OnCreatedNpgsqlCommand();

            return this.CreateCommand(this.Database.Mapping, sqlCommand);
        }

        internal void Commit()
        {
            try
            {
                if (this.NpgsqlTransaction != null)
                {
                    this.OnCommitting();
                    this.NpgsqlTransaction.Commit();
                    this.OnCommitted();
                }
            }
            finally
            {
                this.NpgsqlTransaction = null;

                if (this.NpgsqlConnection != null)
                {
                    this.OnClosingNpgsqlConnection();
                    this.NpgsqlConnection?.Close();
                    this.OnClosedNpgsqlConnection();
                }

                this.NpgsqlConnection = null;
            }
        }

        internal void Rollback()
        {
            try
            {
                if (this.NpgsqlTransaction != null)
                {
                    this.OnRollingBack();
                    this.NpgsqlTransaction?.Rollback();
                    this.OnRolledBack();
                }
            }
            finally
            {
                this.NpgsqlTransaction = null;

                if (this.NpgsqlConnection != null)
                {
                    this.OnClosingNpgsqlConnection();
                    this.NpgsqlConnection?.Close();
                    this.OnClosedNpgsqlConnection();
                }

                this.NpgsqlConnection = null;
            }
        }

        protected abstract Command CreateCommand(Mapping mapping, NpgsqlCommand sqlCommand);

        #region Events

        protected abstract void OnCreatingNpgsqlConnection();

        protected abstract void OnCreatedNpgsqlConnection();

        protected abstract void OnOpeningNpgsqlConnection();

        protected abstract void OnOpenedNpgsqlConnection();

        protected abstract void OnClosingNpgsqlConnection();

        protected abstract void OnClosedNpgsqlConnection();

        protected abstract void OnCreatingNpgsqlTransaction();

        protected abstract void OnCreatedNpgsqlTransaction();

        protected abstract void OnCommitting();

        protected abstract void OnCommitted();

        protected abstract void OnRollingBack();

        protected abstract void OnRolledBack();

        protected abstract void OnCreatingNpgsqlCommand();

        protected abstract void OnCreatedNpgsqlCommand();

        #endregion Events
    }
}
