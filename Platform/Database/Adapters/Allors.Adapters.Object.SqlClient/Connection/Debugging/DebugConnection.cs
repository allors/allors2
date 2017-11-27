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

namespace Allors.Adapters.Object.SqlClient.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

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

        public override string ToString()
        {
            return $"{this.Commands.Count} commands with {this.Executions.Count()} executions.";
        }

        protected override Command CreateCommand(Mapping mapping, SqlCommand sqlCommand)
        {
            var command = new DebugCommand(mapping, sqlCommand);
            this.Commands.Add(command);
            return command;
        }

        #region Events
        protected override void OnCreatingSqlConnection()
        {
        }

        protected override void OnCreatedSqlConnection()
        {
        }

        protected override void OnOpeningSqlConnection()
        {
        }

        protected override void OnOpenedSqlConnection()
        {
        }

        protected override void OnClosingSqlConnection()
        {
        }

        protected override void OnClosedSqlConnection()
        {
        }

        protected override void OnCreatingSqlTransaction()
        {
        }

        protected override void OnCreatedSqlTransaction()
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

        protected override void OnCreatingSqlCommand()
        {
        }

        protected override void OnCreatedSqlCommand()
        {
        }
        #endregion
    }
}
