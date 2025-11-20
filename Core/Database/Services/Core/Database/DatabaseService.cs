// <copyright file="DatabaseService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    public class DatabaseService : IDatabaseService
    {
        public IDatabase Database { get; set; }
    }
}
