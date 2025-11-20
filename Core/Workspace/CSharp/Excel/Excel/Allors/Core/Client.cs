// <copyright file="Client.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using Workspace;

    public class Client
    {
        public Client(IDatabase database, IWorkspace workspace)
        {
            this.Database = database;
            this.Workspace = workspace;
        }

        public IDatabase Database { get; }

        public IWorkspace Workspace { get; }

        public bool IsLoggedIn { get; set; }

        public string UserName { get; set; }
    }
}
