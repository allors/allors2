// <copyright file="StringExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static partial class StringExtensions
    {
        private static readonly HashSet<string> ReservedKeywords = new HashSet<string>
            {
                "abstract",
                "as",
                "base",
                "bool",
                "break",
                "byte",
                "case",
                "catch",
                "char",
                "checked",
                "class",
                "const",
                "continue",
                "decimal",
                "default",
                "delegate",
                "do",
                "double",
                "else",
                "enum",
                "event",
                "explicit",
                "extern",
                "false",
                "finally",
                "fixed",
                "float",
                "for",
                "foreach",
                "goto",
                "if",
                "implicit",
                "in",
                "int",
                "interface",
                "internal",
                "is",
                "lock",
                "long",
                "namespace",
                "new",
                "null",
                "object",
                "operator",
                "out",
                "override",
                "params",
                "private",
                "protected",
                "public",
                "readonly",
                "ref",
                "return",
                "sbyte",
                "sealed",
                "short",
                "sizeof",
                "stackalloc",
                "static",
                "string",
                "struct",
                "switch",
                "this",
                "throw",
                "true",
                "try",
                "typeof",
                "uint",
                "ulong",
                "unchecked",
                "unsafe",
                "ushort",
                "using",
                "using",
                "static",
                "virtual",
                "void",
                "volatile",
                "while",
            };

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

        public static string EscapeReservedKeyword(this string value) => ReservedKeywords.Contains(value) ? $"@{value}" : value;
    }
}
