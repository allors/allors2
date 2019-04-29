// <copyright file="StringExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Allors.Meta;
    using Autotest.Angular;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    public static partial class StringExtensions
    {
        public static string RemoveWhitespace(this string value) {
            return string.Join("", value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
