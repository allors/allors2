// <copyright file="IPredicate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Protocol.Data;

    public interface IPredicate
    {
        string[] Dependencies { get; }

        Predicate Save();

        void Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate);

        bool ShouldTreeShake(IDictionary<string, string> parameters);

        bool HasMissingArguments(IDictionary<string, string> parameters);
    }
}
