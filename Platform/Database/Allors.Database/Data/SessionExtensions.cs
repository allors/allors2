// <copyright file="SessionExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System;
    using System.Linq;

    using Allors.Meta;

    internal static class SessionExtensions
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
            if (value == null)
            {
                return null;
            }

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

            if (value == null)
            {
                return emptyArray;
            }

            if (value is string idAsString)
            {
                return @this.GetObjects(idAsString.Split(','));
            }

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

                default:
                    throw new ArgumentException();
            }
        }
    }
}
