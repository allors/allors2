// <copyright file="StringExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest
{
    using System;
    using System.Linq;

    public static partial class StringExtensions
    {
        public static string RemoveWhitespace(this string value)
        {
            return string.Join(string.Empty, value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public static string Capitalize(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value.First().ToString().ToUpper() + value.Substring(1);
            }

            return value;
        }
    }
}
