// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fixture.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
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

using Allors.Domain;

namespace Allors
{
    public class CustomListDerivationLog : Allors.Domain.Logging.ListDerivationLog
    {
        public Allors.Domain.Logging.Derivation Derivation { get; set; }

        public override void AddedDerivable(Object derivable)
        {
            base.AddedDerivable(derivable);

            if (derivable.Id == 787812)
            {
                var setBreakpointHere = derivable;  // Set a breakpoint here to debug why derivable was added
            }
        }

        public override void AddedDependency(Object dependent, Object dependee)
        {
            base.AddedDependency(dependent, dependee);

            if (dependent.Id == 787812 || dependee.Id == 787812)
            {
                var setBreakpointHere = (dependent.Id == 787812) ? dependent : dependee;  // Set breakpoint here to debug why dependency was added
            }
        }
    }
}