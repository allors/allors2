// <copyright file="StringExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>



namespace Autotest
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public static partial class StringExtensions
    {
        private static Regex NonAlphaNumericRegex => new Regex("[^a-zA-Z0-9]");

        public static string Capitalize(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value.First().ToString().ToUpper() + value.Substring(1);
            }

            return value;
        }

        public static string ToAlphaNumeric(this string value) => !string.IsNullOrEmpty(value) ? NonAlphaNumericRegex.Replace(value, string.Empty) : value;

        public static string EmptyToNull(this string value) => string.IsNullOrEmpty(value) ? null : value;
    }
}
