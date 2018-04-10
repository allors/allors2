// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredicateExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Query
{
    using System;
    using System.Collections;
    using System.Linq;

    public static class PredicateExtensions
    {
        public static void AssertExists<T>(this T @this, QueryValidation validation, string message, Func<T, object> property)
            where T : Predicate
        {
            if (property(@this) == null)
            {
                validation.AddError($"{@this}: {message}");
            }
        }

        public static void AssertAtLeastOne<T>(this T @this, QueryValidation validation, string message, params Func<T, object>[] properties)
            where T : Predicate
        {
            if (properties.Count(v=>
                {
                    var result = v(@this);
                    var collection = result as ICollection;
                    if(collection != null)
                    {
                        return collection.Count > 0;
                    }

                    return result != null ;
                }) < 1)
            {
                validation.AddError($"{@this}: {message}");
            }
        }
    }
}