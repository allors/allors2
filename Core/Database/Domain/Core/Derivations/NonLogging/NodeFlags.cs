// <copyright file="NodeFlags.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    using System;

    [Flags]
    public enum Flags : byte
    {
        IsMarked = 1,
        IsScheduled = 2,
        IsVisited = 4,
    }
}
