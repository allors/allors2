//-------------------------------------------------------------------------------------------------
// <copyright file="IPredicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Protocol.Data;

    public interface IPredicate
    {
        Predicate Save();

        void Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate);

        bool ShouldTreeShake(IReadOnlyDictionary<string, object> arguments);

        bool HasMissingArguments(IReadOnlyDictionary<string, object> arguments);
    }
}
