// <copyright file="Derivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Object = Domain.Object;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Allors Object")]
    public sealed class Derivation : IDerivation
    {
        public IAccumulatedChangeSet ChangeSet { get; }

        public object this[string name]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public Guid Id { get; }
        public DateTime TimeStamp { get; }
        public ISession Session { get; }
        public IValidation Validation { get; }
        public ICycle Cycle { get; }
        public ISet<Object> DerivedObjects { get; }
        public IValidation Derive() => throw new NotImplementedException();

        public IValidation Derive(params Object[] marked) => throw new NotImplementedException();
    }
}
