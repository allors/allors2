// <copyright file="Connection.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    using System.Data.SqlClient;

    public abstract class Connection
    {
        protected readonly Database Database;

        protected Connection(Database database) => this.Database = database;

        protected SqlConnection SqlConnection { get; private set; }

        protected SqlTransaction SqlTransaction { get; private set; }

        internal Command CreateCommand()
        {
            if (this.SqlConnection == null)
            {
                this.OnCreatingSqlConnection();
                this.SqlConnection = new SqlConnection(this.Database.ConnectionString);
                this.OnCreatedSqlConnection();

                this.OnOpeningSqlConnection();
                this.SqlConnection.Open();
                this.OnOpenedSqlConnection();

                this.OnCreatingSqlTransaction();
                this.SqlTransaction = this.SqlConnection.BeginTransaction(this.Database.IsolationLevel ?? Database.DefaultIsolationLevel);
                this.OnCreatedSqlTransaction();
            }

            this.OnCreatingSqlCommand();
            var sqlCommand = this.SqlConnection.CreateCommand();
            sqlCommand.Transaction = this.SqlTransaction;
            if (this.Database.CommandTimeout.HasValue)
            {
                sqlCommand.CommandTimeout = this.Database.CommandTimeout.Value;
            }

            this.OnCreatedSqlCommand();

            return this.CreateCommand(this.Database.Mapping, sqlCommand);
        }

        internal void Commit()
        {
            try
            {
                if (this.SqlTransaction != null)
                {
                    this.OnCommitting();
                    this.SqlTransaction.Commit();
                    this.OnCommitted();
                }
            }
            finally
            {
                this.SqlTransaction = null;

                if (this.SqlConnection != null)
                {
                    this.OnClosingSqlConnection();
                    this.SqlConnection?.Close();
                    this.OnClosedSqlConnection();
                }

                this.SqlConnection = null;
            }
        }

        internal void Rollback()
        {
            try
            {
                if (this.SqlTransaction != null)
                {
                    this.OnRollingBack();
                    this.SqlTransaction?.Rollback();
                    this.OnRolledBack();
                }
            }
            finally
            {
                this.SqlTransaction = null;

                if (this.SqlConnection != null)
                {
                    this.OnClosingSqlConnection();
                    this.SqlConnection?.Close();
                    this.OnClosedSqlConnection();
                }

                this.SqlConnection = null;
            }
        }

        protected abstract Command CreateCommand(Mapping mapping, SqlCommand sqlCommand);

        #region Events

        protected abstract void OnCreatingSqlConnection();

        protected abstract void OnCreatedSqlConnection();

        protected abstract void OnOpeningSqlConnection();

        protected abstract void OnOpenedSqlConnection();

        protected abstract void OnClosingSqlConnection();

        protected abstract void OnClosedSqlConnection();

        protected abstract void OnCreatingSqlTransaction();

        protected abstract void OnCreatedSqlTransaction();

        protected abstract void OnCommitting();

        protected abstract void OnCommitted();

        protected abstract void OnRollingBack();

        protected abstract void OnRolledBack();

        protected abstract void OnCreatingSqlCommand();

        protected abstract void OnCreatedSqlCommand();

        #endregion Events
    }
}
