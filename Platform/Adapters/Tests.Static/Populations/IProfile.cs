
// <copyright file="IProfile.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors;

namespace Allors.Adapters
{
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
