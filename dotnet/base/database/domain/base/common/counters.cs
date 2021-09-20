// <copyright file="Counters.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
                counter.Value += 1;

                while (!IsValidElfProefNumber(counter.Value))
                {
                    counter.Value += 1;
                }

                return counter.Value;
            }

            if (session.Database.IsShared)
            {
                using (var outOfBandSession = session.Database.CreateSession())
                {
                    var outOfBandCounter = new Counters(outOfBandSession).Cache[counterId];
                    if (outOfBandCounter != null)
                    {
                        var value = NextElfProefValue(outOfBandCounter);
                        outOfBandSession.Commit();
                        return value;
                    }
                }
            }

            var sessionCounter = new Counters(session).Cache[counterId];
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
            if (total == 0)
                return false;

            // ... and not have a modulo when dividing by 11.
            return total % 11 == 0;
        }
    }
}
