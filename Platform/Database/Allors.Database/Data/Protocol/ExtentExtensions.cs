// <copyright file="ExtentExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
            IExtent[] Operands() => @this.Operands.Select(v => v.Load(session)).ToArray();

            IExtent extent;

            switch (@this.Kind)
            {
                case ExtentKind.Extent:
                    if (!@this.ObjectType.HasValue)
                    {
                        return null;
                    }

                    var objectType = (IComposite)session.Database.ObjectFactory.MetaPopulation.Find(@this.ObjectType.Value);
                    extent = new Allors.Data.Extent(objectType)
                    {
                        Predicate = @this.Predicate?.Load(session),
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
                    throw new Exception("Unknown extent kind " + @this.Kind);
            }

            extent.Sorting = @this.Sorting?.Select(v => v.Load(session)).ToArray();
            return extent;
        }
    }
}
