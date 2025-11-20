// <copyright file="IDerivation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using Allors;
    using Derivations;

    public interface IDerivation
    {
        IAccumulatedChangeSet ChangeSet { get; }

        object this[string name] { get; set; }

        Guid Id { get; }

        DateTime TimeStamp { get; }

        ISession Session { get; }

        IValidation Validation { get; }

        ICycle Cycle { get; }

        ISet<Domain.Object> DerivedObjects { get; }

        IValidation Derive();

        void Mark(Domain.Object @object);

        void Mark(params Domain.Object[] objects);
    }
}
