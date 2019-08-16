// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Counters.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
    using Allors;

    public partial class Counters
    {
        public static int NextElfProefValue(ISession session, Guid counterId)
        {
            int NextElfProefValue(Counter counter)
            {
                counter.Value = counter.Value + 1;

                while (!IsValidElfProefNumber(counter.Value))
                {
                    counter.Value = counter.Value + 1;
                }

                return counter.Value;
            }

            if (session.Database.IsShared)
            {
                using (var outOfBandSession = session.Database.CreateSession())
                {
                    var outOfBandCounter = new Counters(outOfBandSession).Sticky[counterId];
                    if (outOfBandCounter != null)
                    {
                        var value = NextElfProefValue(outOfBandCounter);
                        outOfBandSession.Commit();
                        return value;
                    }
                }
            }

            var sessionCounter = new Counters(session).Sticky[counterId];
            return NextElfProefValue(sessionCounter);
        }

        public static bool IsValidElfProefNumber(int number)
        {
            var numberString = number.ToString();
            var length = numberString.Length;

            // ... the number must be validatable to the so-called 11-proof ...
            long total = 0;
            for (var i = 0; i <= numberString.Length - 1; i++)
            {
                var nummertje = Convert.ToInt32(numberString[i].ToString());
                total += nummertje * length;
                length--;
            }

            // ... not result in a 0 when dividing by 11 ...
            if (total == 0) return false;

            // ... and not have a modulo when dividing by 11.
            return total % 11 == 0;
        }
    }
}
