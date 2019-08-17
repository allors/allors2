// <copyright file="DebugConnection.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql.Debug
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Npgsql;

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

        #endregion Events
    }
}
