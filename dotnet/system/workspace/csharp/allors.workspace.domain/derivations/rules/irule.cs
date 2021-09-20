// <copyright file="IDomainDerivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Workspace.Derivations
{
    using System;
    using System.Collections.Generic;

    public interface IRule
    {
        Guid Id { get; }

        IEnumerable<IPattern> Patterns { get; }

        void Derive(ICycle cycle, IEnumerable<IObject> matches);
    }
}
