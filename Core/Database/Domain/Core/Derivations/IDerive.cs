// <copyright file="IValidation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public interface IDerive
    {
        IAccumulatedChangeSet ChangeSet { get; }

        object this[string name] { get; set; }
    }
}
