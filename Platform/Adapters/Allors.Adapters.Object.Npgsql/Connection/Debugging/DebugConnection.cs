// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingConnection.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.Npgsql.Debug
{
    using System.Collections.Generic;
    using global::Npgsql;
    using System.Linq;

    public class DebugConnection : Connection
    {
        public List<DebugCommand> Commands { get; } = new List<DebugCommand>();

        public IEnumerable<DebugExecution> Executions =>
            from command in this.Commands
            from execution in command.Executions
            orderby execution.Begin
            select execution;

        public List<DebugExecution> ExecutionList => this.Executions.ToList();

        public DebugConnection(Database database)
            : base(database)
        {
        }

        public override string ToString() => $"{this.Commands.Count} commands with {this.Executions.Count()} executions.";

        protected override Command CreateCommand(Mapping mapping, NpgsqlCommand sqlCommand)
        {
            var command = new DebugCommand(mapping, sqlCommand);
            this.Commands.Add(command);
            return command;
        }

        #region Events
        protected override void OnCreatingNpgsqlConnection()
        {
        }

        protected override void OnCreatedNpgsqlConnection()
        {
        }

        protected override void OnOpeningNpgsqlConnection()
        {
        }

        protected override void OnOpenedNpgsqlConnection()
        {
        }

        protected override void OnClosingNpgsqlConnection()
        {
        }

        protected override void OnClosedNpgsqlConnection()
        {
        }

        protected override void OnCreatingNpgsqlTransaction()
        {
        }

        protected override void OnCreatedNpgsqlTransaction()
        {
        }

        protected override void OnCommitting()
        {
        }

        protected override void OnCommitted()
        {
        }

        protected override void OnRollingBack()
        {
        }

        protected override void OnRolledBack()
        {
        }

        protected override void OnCreatingNpgsqlCommand()
        {
        }

        protected override void OnCreatedNpgsqlCommand()
        {
        }
        #endregion
    }
}
