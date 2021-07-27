// <copyright file="Constants.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Application.Excel
{
    using System.Drawing;
    using Allors.Excel;

    public static class Constants
    {
        public const string YES = "YES";
        public const string NO = "NO";

        public static readonly Style ChangedStyle = new Style(Color.DeepSkyBlue, Color.Black);

        public static readonly Style ReadOnlyStyle = new Style(Color.FromArgb(253, 233, 217), Color.Black);

        public static readonly Style WriteStyle = new Style(Color.LightBlue, Color.Black);
    }
}
