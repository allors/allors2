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

namespace Allors.Adapters.Object.Npgsql.Debug
{
    using System;

    public class DebugExecution
    {
        public DebugExecution(DebugCommand debugCommand)
        {
            this.Command = debugCommand;
        }

        public DebugCommand Command { get; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public override string ToString()
        {
            return $"{this.Begin:hh:mm:ss.ff} {this.Command}";
        }
    }
}
