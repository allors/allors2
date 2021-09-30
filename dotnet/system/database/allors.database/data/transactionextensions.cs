// <copyright file="TransactionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System;
    using System.Linq;

    using Meta;

    internal static class TransactionExtensions
    {
        internal static IMetaObject GetMetaObject(this ITransaction @this, object value) =>
            value switch
            {
                IComposite metaObject => metaObject,
                string idAsString when Guid.TryParse(idAsString, out var idFromString) => @this.Database.MetaPopulation.FindById(idFromString),
                Guid idAsGuid => @this.Database.MetaPopulation.FindById(idAsGuid),
                string tag => @this.Database.MetaPopulation.FindByTag(tag),
                _ => throw new ArgumentException()
            };

        internal static IObject GetObject(this ITransaction @this, object value) =>
            value switch
            {
                IObject @object => @object,
                long idAsLong => @this.Instantiate(idAsLong),
                string idAsString => @this.Instantiate(idAsString),
                null => null,
                _ => throw new ArgumentException()
            };

        internal static IObject[] GetObjects(this ITransaction @this, object value)
        {
            switch (value)
            {
                case null:
                    return Array.Empty<IObject>();
                case string idAsString:
                    return @this.GetObjects(idAsString.Split(','));
                default:
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
                            return objectFromLong != null ? new[] { objectFromLong } : Array.Empty<IObject>();

                        default:
                            throw new ArgumentException();
                    }
            }
        }
    }
}
