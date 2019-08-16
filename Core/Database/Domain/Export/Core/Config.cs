// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System.IO;

    public partial class Config
    {
        public DirectoryInfo DataPath { get; set; }

        public bool SetupSecurity { get; set; } = true;

        public bool Demo { get; set; } = false;

        public bool Unit { get; set; } = false;

        public bool End2End { get; set; } = false;
    }
}
