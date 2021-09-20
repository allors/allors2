// <copyright file="TreeNodeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public static class IPredicateExtensions
    {
        public static bool HasMissingDependencies(this IPredicate @this, IDictionary<string, string> parameters)
        {
            if (@this.Dependencies?.Length > 0)
            {
                if(parameters == null)
                {
                    return true;
                }

                return !@this.Dependencies.All(v => parameters.ContainsKey(v));
            }

            return false;
        }
    }
}
