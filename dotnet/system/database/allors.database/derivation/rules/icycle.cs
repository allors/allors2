// <copyright file="ICycle.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Derivations
{
    public interface ICycle
    {
        ITransaction Transaction { get; }

        IChangeSet ChangeSet { get; }

        IValidation Validation { get; }
    }
}
