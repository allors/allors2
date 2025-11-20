// <copyright file="IStateService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    public interface IStateService
    {
        void Register(IStateful stateful);

        void Clear();
    }
}
