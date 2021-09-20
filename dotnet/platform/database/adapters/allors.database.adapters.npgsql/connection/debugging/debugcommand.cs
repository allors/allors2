// <copyright file="DebugCommand.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql.Debug
{
    using System;
    using System.Collections.Generic;
    using global::Npgsql;

    public class DebugCommand : Command
    {
        public DebugCommand(Mapping mapping, NpgsqlCommand command)
            : base(mapping, command)
        {
        }

        public List<DebugExecution> Executions { get; } = new List<DebugExecution>();

        public DebugExecution CurrentExecution { get; set; }

        public override string ToString() => $"[{this.Executions.Count}x] {this.NpgsqlCommand.CommandText}";

        protected override void OnExecuting()
        {
            this.CurrentExecution = new DebugExecution(this) { Begin = DateTime.Now };
            this.Executions.Add(this.CurrentExecution);
        }

        protected override void OnExecuted()
        {
            if (this.CurrentExecution == null)
            {
                this.CurrentExecution = new DebugExecution(this) { Begin = DateTime.Now };
                this.Executions.Add(this.CurrentExecution);
            }

            this.CurrentExecution.End = DateTime.Now;
        }
    }
}
