// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugCommand.cs" company="Allors bvba">
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

    public class DebugCommand : Command
    {
        public List<DebugExecution> Executions { get; } = new List<DebugExecution>();

        public DebugExecution CurrentExecution { get; set; }

        public DebugCommand(Mapping mapping, SqlCommand command)
            : base(mapping, command)
        {
        }

        public override string ToString() => $"[{this.Executions.Count}x] {this.SqlCommand.CommandText}";

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
