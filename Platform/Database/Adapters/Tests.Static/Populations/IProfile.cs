// <copyright file="IProfile.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using Allors;
    using System;

    public interface IProfile : IDisposable
    {
        IDatabase Database { get; }

        ISession Session { get; }

        Action[] Markers { get; }

        Action[] Inits { get; }

        IObjectFactory ObjectFactory { get; }

        IDatabase CreateDatabase();
    }
}
