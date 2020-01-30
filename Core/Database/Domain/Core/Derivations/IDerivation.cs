// <copyright file="IDerivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using Allors;

    public interface IDerivation
    {
        IAccumulatedChangeSet ChangeSet { get; }

        object this[string name] { get; set; }

        Guid Id { get; }

        DateTime TimeStamp { get; }

        ISession Session { get; }

        IValidation Validation { get; }

        ICycle Cycle { get; }

        ISet<Object> DerivedObjects { get; }

        IValidation Derive();

        void Mark(Object @object);

        void Mark(params Object[] objects);
    }
}
