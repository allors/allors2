// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFactory.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    public static partial class DateTimeFactory
    {
        public static DateTime CreateDate(int year, int month, int day)
        {
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }

        public static DateTime CreateDateTime(int year, int month, int day, int hour, int min, int sec, int milliSec)
        {
            return new DateTime(year, month, day, hour, min, sec, milliSec, DateTimeKind.Utc);
        }

        public static DateTime CreateDate(DateTime dateTime)
        {
            var utcDateTime = dateTime.ToUniversalTime();
            return CreateDate(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day);
        }

        public static DateTime CreateDateTime(DateTime dateTime)
        {
            var utcDateTime = dateTime.ToUniversalTime();
            return CreateDateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, utcDateTime.Hour, utcDateTime.Minute, utcDateTime.Second, utcDateTime.Millisecond);
        }

        public static DateTime? CreateDate(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                var utcDateTime = dateTime.Value.ToUniversalTime();
                return CreateDate(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day);
            }

            return null;
        }

        public static DateTime? CreateDateTime(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                var utcDateTime = dateTime.Value.ToUniversalTime();
                return CreateDateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, utcDateTime.Hour, utcDateTime.Minute, utcDateTime.Second, utcDateTime.Millisecond);
            }

            return null;
        }
    }
}