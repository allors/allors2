
// <copyright file="IFetchService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System;

    using Allors.Data;

    public partial interface IFetchService
    {
        Fetch Get(Guid id);
    }
}
