// <copyright file="SessionService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System;

    public partial class SessionService : ISessionService, IDisposable
    {
        public SessionService(IDatabaseService databaseService) => this.Session = databaseService.Database.CreateSession();

        public ISession Session { get; private set; }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }
    }
}
