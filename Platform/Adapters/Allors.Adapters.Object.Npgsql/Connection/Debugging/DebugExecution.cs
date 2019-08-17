// <copyright file="DebugExecution.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql.Debug
{
    using System;

    public class DebugExecution
    {
        public DebugExecution(DebugCommand debugCommand) => this.Command = debugCommand;

        public DebugCommand Command { get; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public override string ToString() => $"{this.Begin:hh:mm:ss.ff} {this.Command}";
    }
}
