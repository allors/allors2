// <copyright file="SessionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System;
    using System.Linq;

    using Allors.Meta;

    public static class SessionExtensions
    {
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
            var emptyArray = Array.Empty<IObject>();
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
                    return objectFromLong != null ? new[] { objectFromLong } : emptyArray;

                case string idAsString:
                    var objectFromString = @this.Instantiate(idAsString);
                    return objectFromString != null ? new[] { objectFromString } : emptyArray;

                default:
                    throw new ArgumentException();
            }
        }
    }
}
