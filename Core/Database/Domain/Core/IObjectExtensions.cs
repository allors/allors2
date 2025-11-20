// <copyright file="ObjectExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    public static partial class IObjectExtensions
    {
        public static ISession Session(this IObject @this) => @this.Strategy.Session;
    }
}
