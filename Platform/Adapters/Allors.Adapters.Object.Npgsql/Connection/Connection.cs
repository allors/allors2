// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Connection.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using global::Npgsql;

    public abstract class Connection
    {
        protected readonly Database Database;

        protected NpgsqlConnection NpgsqlConnection { get; private set; }

        protected NpgsqlTransaction NpgsqlTransaction { get; private set; }

        protected Connection(Database database) => this.Database = database;

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
                this.NpgsqlTransaction = this.NpgsqlConnection.BeginTransaction(this.Database.IsolationLevel);
                this.OnCreatedNpgsqlTransaction();
            }

            this.OnCreatingNpgsqlCommand();
            var sqlCommand = this.NpgsqlConnection.CreateCommand();
            sqlCommand.Transaction = this.NpgsqlTransaction;
            sqlCommand.CommandTimeout = this.Database.CommandTimeout;
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

        #endregion
    }
}
