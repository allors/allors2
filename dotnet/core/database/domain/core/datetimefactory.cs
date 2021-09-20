// <copyright file="DateTimeFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public static partial class DateTimeFactory
    {
        public static DateTime CreateDate(int year, int month, int day) => new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime CreateDateTime(int year, int month, int day, int hour, int min, int sec, int milliSec) => new DateTime(year, month, day, hour, min, sec, milliSec, DateTimeKind.Utc);

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
