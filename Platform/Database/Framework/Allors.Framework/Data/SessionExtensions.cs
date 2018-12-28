//------------------------------------------------------------------------------------------------- 
// <copyright file="SessionExtensions.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System;
    using System.Linq;

    using Allors.Meta;

    public static class SessionExtensions
    {
        private static readonly IObject[] EmptyArray = { };

        internal static IMetaObject GetMetaObject(this ISession @this, object value)
        {
            switch (value)
            {
                case IComposite metaObject:
                    return metaObject;

                case Guid idAsGuid:
                    return @this.Database.MetaPopulation.Find(idAsGuid);

                case string idAsString:
                    return @this.Database.MetaPopulation.Find(new Guid(idAsString));

                default:
                    throw new ArgumentException();
            }
        }

        internal static IObject GetObject(this ISession @this, object value)
        {
            switch (value)
            {
                case IObject @object:
                    return @object;

                case long idAsLong:
                    return @this.Instantiate(idAsLong);

                case string idAsString:
                    return @this.Instantiate(idAsString);

                default:
                    throw new ArgumentException();
            }
        }

        internal static IObject[] GetObjects(this ISession @this, object value)
        {
            switch (value)
            {
                case IObject[] objects:
                    return objects;

                case long[] idAsLongs:
                    return idAsLongs.Select(@this.Instantiate).Where(v => v != null).ToArray();

                case string[] idAsStrings:
                    return idAsStrings.Select(@this.Instantiate).Where(v => v != null).ToArray();

                case IObject @object:
                    return new[] { @object };

                case long idAsLong:
                    var objectFromLong = @this.Instantiate(idAsLong);
                    return objectFromLong != null ? new[] { objectFromLong } : EmptyArray;

                case string idAsString:
                    var objectFromString = @this.Instantiate(idAsString);
                    return objectFromString != null ? new[] { objectFromString } : EmptyArray;

                default:
                    throw new ArgumentException();
            }
        }
    }
}