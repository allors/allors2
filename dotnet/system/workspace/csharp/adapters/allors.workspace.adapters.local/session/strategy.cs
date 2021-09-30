// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Local
{
    using Meta;

    public sealed class Strategy : Adapters.Strategy
    {
        internal Strategy(Adapters.Session session, IClass @class, long id) : base(session, @class, id)
        {
            if (this.Class.Origin == Origin.Database)
            {
                this.DatabaseOriginState = new DatabaseOriginState(this, session.Workspace.DatabaseConnection.GetRecord(this.Id));
            }
        }

        internal Strategy(Adapters.Session session, Adapters.DatabaseRecord databaseRecord) : base(session, databaseRecord) => this.DatabaseOriginState = new DatabaseOriginState(this, databaseRecord);
    }
}
