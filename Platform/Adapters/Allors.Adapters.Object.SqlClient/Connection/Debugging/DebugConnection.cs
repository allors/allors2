// <copyright file="DebugConnection.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.SqlClient.Debug
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class DebugConnection : Connection
    {
        public DebugConnection(Database database)
            : base(database)
        {
        }

        public List<DebugCommand> Commands { get; } = new List<DebugCommand>();

        public IEnumerable<DebugExecution> Executions =>
            from command in this.Commands
            from execution in command.Executions
            orderby execution.Begin
            select execution;

        public List<DebugExecution> ExecutionList => this.Executions.ToList();

        public override string ToString() => $"{this.Commands.Count} commands with {this.Executions.Count()} executions.";

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

        #endregion Events
    }
}
