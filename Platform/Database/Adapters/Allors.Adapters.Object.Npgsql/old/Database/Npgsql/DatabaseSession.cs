//------------------------------------------------------------------------------------------------- 
// <copyright file="DatabaseSession.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
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
// <summary>Defines the Session type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Npgsql
{
    using System.Collections.Generic;

    using Allors.Adapters.Database.Sql;

    using global::Npgsql;

    using IDatabase = Allors.IDatabase;

    public class DatabaseSession : Sql.DatabaseSession, ICommandFactory
    {
        private readonly Database database;

        private NpgsqlConnection connection;
        private NpgsqlTransaction transaction;

        private SessionCommands sessionCommands;

        internal DatabaseSession(Database database)
        : base(database)
        {
            this.database = database;
        }
        
        public override IDatabase Database
        {
            get { return this.database; }
        }

        public override Sql.Database SqlDatabase
        {
            get { return this.database; }
        }

        public Database NpgsqlDatabase
        {
            get { return this.database; }
        }

        public override Sql.SessionCommands SessionCommands
        {
            get
            {
                return this.sessionCommands ?? (this.sessionCommands = new SessionCommands(this));
            }
        }

        public virtual NpgsqlCommand CreateNpgsqlCommand(string commandText)
        {
            var command = this.CreateNpgsqlCommand();
            command.CommandText = commandText;
            return command;
        }

        public virtual NpgsqlCommand CreateNpgsqlCommand()
        {
            if (this.connection == null)
            {
                this.connection = new NpgsqlConnection(this.SqlDatabase.ConnectionString);
                this.connection.Open();
                this.transaction = this.connection.BeginTransaction(this.SqlDatabase.IsolationLevel);
            }

            var command = this.connection.CreateCommand();
            command.Transaction = this.transaction;
            command.CommandTimeout = this.SqlDatabase.CommandTimeout;
            return command;
        }

        public override Sql.Command CreateCommand(string commandText)
        {
            return new Command(this, commandText);
        }

        protected override IFlush CreateFlush(Dictionary<Reference, Roles> unsyncedRolesByReference)
        {
            return new Flush(this, unsyncedRolesByReference);
        }

        protected override void SqlCommit()
        {
            try
            {
                this.sessionCommands = null;
                transaction?.Commit();
            }
            finally
            {
                this.transaction = null;
                connection?.Close();
                this.connection = null;
            }
        }

        protected override void SqlRollback()
        {
            try
            {
                this.sessionCommands = null;
                transaction?.Rollback();
            }
            finally
            {
                this.transaction = null;
                connection?.Close();
                this.connection = null;
            }
        }
    }
}