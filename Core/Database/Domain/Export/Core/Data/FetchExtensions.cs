// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FetchExtensions.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using Allors.Data;

    public static class FetchExtensions
    {
        public static object Get(this Fetch fetch, IObject allorsObject, IAccessControlListFactory aclFactory)
        {
            return fetch.Step == null ? allorsObject : fetch.Step.Get(allorsObject, aclFactory);
        }

        public static bool Set(this Fetch fetch, IObject allorsObject, IAccessControlListFactory aclFactory, object value)
        {
            return fetch.Step != null && fetch.Step.Set(allorsObject, aclFactory, value);
        }

        public static void Ensure(this Fetch fetch, IObject allorsObject, IAccessControlListFactory aclFactory)
        {
            fetch.Step.Ensure(allorsObject, aclFactory);
        }
    }
}
