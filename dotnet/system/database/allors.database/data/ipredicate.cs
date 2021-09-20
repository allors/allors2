// <copyright file="IPredicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    public interface IPredicate : IVisitable
    {
        string[] Dependencies { get; }

        void Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate);

        bool ShouldTreeShake(IArguments arguments);

        bool HasMissingArguments(IArguments arguments);
    }
}
