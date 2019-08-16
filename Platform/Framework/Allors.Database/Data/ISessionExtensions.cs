//-------------------------------------------------------------------------------------------------
// <copyright file="ISessionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ISessionExtensions
    {
        public static T[] Resolve<T>(this ISession session, IExtent extent, IReadOnlyDictionary<string, object> arguments = null) where T : IObject => extent.Build(session, arguments).Cast<T>().ToArray();

        public static T[] Resolve<T>(this ISession session, IExtent extent, object arguments) where T : IObject => extent.Build(session, new Arguments(arguments)).Cast<T>().ToArray();
    }
}
