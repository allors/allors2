// <copyright file="DatabaseService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    public class DatabaseService : IDatabaseService
    {
        public IDatabase Database { get; set; }
    }
}
