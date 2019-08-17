// <copyright file="For.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Document.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class For : Statement
    {
        public static readonly HashSet<string> ReservedNames = new HashSet<string>
                                                                   {
                                                                       "default",
                                                                       "first",
                                                                       "group",
                                                                       "if",
                                                                       "implements",
                                                                       "interface",
                                                                       "last",
                                                                       "length",
                                                                       "optional",
                                                                       "rest",
                                                                       "strip",
                                                                       "super",
                                                                       "trunc",
                                                                       "else",
                                                                       "endif",
                                                                       "elseif",
                                                                       "i", // 1 based index argument
                                                                       "i0", // 0 based index argument
                                                                   };

        private static readonly Regex Regex = new Regex(@"(\w+)(.*)", RegexOptions.IgnoreCase);

        public For(int index, string text)
        {
            this.Index = index;

            var match = Regex.Match(text);
            if (!match.Success)
            {
                throw new ArgumentException("text");
            }

            this.Argument = match.Groups[1].ToString().Trim();
            this.Expression = match.Groups[2].ToString().Trim();
            this.Statement = $"for{index}({this.Argument})";

            if (ReservedNames.Contains(this.Argument))
            {
                throw new ArgumentException(this.Argument + " is a reserved name for a template argument");
            }
        }

        public int Index { get; }

        public string Expression { get; }

        public string Argument { get; }

        public string Statement { get; }
    }
}
