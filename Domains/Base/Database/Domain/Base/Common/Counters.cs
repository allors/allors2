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

namespace Allors.Domain
{
    using System;
    using System.Threading;

    using Allors;

    public partial class Counters
    {
        private UniquelyIdentifiableCache<Counter> cache;
        
        private UniquelyIdentifiableCache<Counter> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<Counter>(this.Session));
        
        public static int NextValue(ISession session, Guid counterId)
        {
            var serializable = session.Database.Serializable;
            if (serializable != null)
            {
                using (var counterSession = serializable.CreateSession())
                {
                    var serializableCounter = new Counters(counterSession).Cache[counterId];
                    var newValue = serializableCounter.Value + 1;
                    serializableCounter.Value = newValue;

                    counterSession.Commit();

                    return newValue;
                }
            }

            var counter = new Counters(session).Cache[counterId];
            counter.Value = counter.Value + 1;

            return counter.Value;
        }
    }
}