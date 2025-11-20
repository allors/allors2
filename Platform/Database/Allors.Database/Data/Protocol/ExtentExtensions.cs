// <copyright file="ExtentExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;
    using System.Linq;

    using Allors.Data;
    using Allors.Meta;

    public static class ExtentExtensions
    {
        /// <summary>
        /// Loads an <see cref="Allors.Extent"/> based on this <see cref="Extent"/>.
        /// </summary>
        /// <param name="@this"></param>
        /// <param name="session">
        /// The database to resolve information from.
        /// </param>
        /// <returns>
        /// The loaded <see cref="Extent"/>.
        /// </returns>
        public static IExtent Load(this Extent @this, ISession session)
        {
            IExtent[] Operands() => @this.operands.Select(v => v.Load(session)).ToArray();

            IExtent extent;

            switch (@this.kind)
            {
                case ExtentKind.Extent:
                    if (!@this.objectType.HasValue)
                    {
                        return null;
                    }

                    var objectType = (IComposite)session.Database.ObjectFactory.MetaPopulation.Find(@this.objectType.Value);
                    extent = new Allors.Data.Extent(objectType)
                    {
                        Predicate = @this.predicate?.Load(session),
                    };

                    break;

                case ExtentKind.Union:
                    extent = new Union(Operands());
                    break;

                case ExtentKind.Except:
                    extent = new Except(Operands());
                    break;

                case ExtentKind.Intersect:
                    extent = new Intersect(Operands());
                    break;

                default:
                    throw new Exception("Unknown extent kind " + @this.kind);
            }

            extent.Sorting = @this.sorting?.Select(v => v.Load(session)).ToArray();
            return extent;
        }
    }
}
