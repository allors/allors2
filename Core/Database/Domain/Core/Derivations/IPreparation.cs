// <copyright file="IValidation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations
{
    using System.Collections.Generic;

    public interface IPreparation
    {
        ISet<Object> Objects { get; }
    }
}
