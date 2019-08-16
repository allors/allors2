// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Counters.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Allors.Domain
{
    using System;

    using Allors;

    public partial class Counters
    {
        private UniquelyIdentifiableSticky<Counter> sticky;

        private UniquelyIdentifiableSticky<Counter> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<Counter>(this.Session));

        public static int NextValue(ISession session, Guid counterId)
        {
            int NextValue(Counter counter)
            {
                counter.Value = counter.Value + 1;
                return counter.Value;
            }

            if (session.Database.IsShared)
            {
                using (var outOfBandSession = session.Database.CreateSession())
                {
                    var outOfBandCounter = new Counters(outOfBandSession).Sticky[counterId];
                    if (outOfBandCounter != null)
                    {
                        var value = NextValue(outOfBandCounter);
                        outOfBandSession.Commit();
                        return value;
                    }
                }
            }

            var sessionCounter = new Counters(session).Sticky[counterId];
            return NextValue(sessionCounter);
        }
    }
}