// <copyright file="Config.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System.IO;

    public partial class Config
    {
        public DirectoryInfo DataPath { get; set; }

        public bool SetupSecurity { get; set; } = true;
    }
}
