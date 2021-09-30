// <copyright file="TreeNodeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System.Linq;

    public static class IPredicateExtensions
    {
        public static bool HasMissingDependencies(this IPredicate @this, IArguments arguments)
        {
            if (@this.Dependencies?.Length > 0)
            {
                if (arguments == null)
                {
                    return true;
                }

                return !@this.Dependencies.All(arguments.HasArgument);
            }

            return false;
        }
    }
}
