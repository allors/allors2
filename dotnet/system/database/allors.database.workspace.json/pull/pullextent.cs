// <copyright file="PullExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Security;
    using Services;
    using Extent = Extent;

    public class PullExtent
    {
        private readonly ITransaction transaction;
        private readonly Pull pull;
        private readonly IAccessControl acls;
        private readonly IPreparedExtents preparedExtents;
        private readonly IPreparedSelects preparedSelects;

        public PullExtent(ITransaction transaction, Pull pull, IAccessControl acls, IPreparedSelects preparedSelects,
            IPreparedExtents preparedExtents)
        {
            this.transaction = transaction;
            this.pull = pull;
            this.acls = acls;
            this.preparedExtents = preparedExtents;
            this.preparedSelects = preparedSelects;
        }

        public void Execute(PullResponseBuilder response)
        {
            if (this.pull.Extent == null && !this.pull.ExtentRef.HasValue)
            {
                throw new Exception("Either an Extent or an ExtentRef is required.");
            }

            var extent = this.pull.Extent ?? this.preparedExtents.Get(this.pull.ExtentRef.Value);
            var objects = extent.Build(this.transaction, this.pull.Arguments).ToArray();

            if (this.pull.Results != null)
            {
                foreach (var result in this.pull.Results)
                {
                    try
                    {
                        var name = result.Name;

                        var select = result.Select;
                        if (select == null && result.SelectRef.HasValue)
                        {
                            select = this.preparedSelects.Get(result.SelectRef.Value);
                        }

                        if (select != null)
                        {
                            var include = select.End.Include;

                            if (select.PropertyType != null)
                            {
                                objects = select.IsOne ?
                                              objects.Select(v => select.Get(v, this.acls)).Where(v => v != null).Cast<IObject>().Distinct().ToArray() :
                                              objects.SelectMany(v =>
                                              {
                                                  var stepResult = select.Get(v, this.acls);
                                                  return stepResult is HashSet<object> set ? set.Cast<IObject>().ToArray() : ((Extent)stepResult)?.ToArray() ?? Array.Empty<IObject>();
                                              }).Distinct().ToArray();

                                var propertyType = select.End.PropertyType;
                                name ??= propertyType.PluralName;
                            }

                            name ??= extent.ObjectType.PluralName;

                            if (result.Skip.HasValue || result.Take.HasValue)
                            {
                                var paged = result.Skip.HasValue ? objects.Skip(result.Skip.Value) : objects;
                                if (result.Take.HasValue)
                                {
                                    paged = paged.Take(result.Take.Value);
                                }

                                paged = paged.ToArray();

                                response.AddValue(name + "_total", extent.Build(this.transaction, this.pull.Arguments).Count.ToString());
                                response.AddCollection(name, paged, include);
                            }
                            else
                            {
                                response.AddCollection(name, objects, include);
                            }
                        }
                        else
                        {
                            name ??= extent.ObjectType.PluralName;
                            var include = result.Include;

                            if (result.Skip.HasValue || result.Take.HasValue)
                            {
                                var paged = result.Skip.HasValue ? objects.Skip(result.Skip.Value) : objects;
                                if (result.Take.HasValue)
                                {
                                    paged = paged.Take(result.Take.Value);
                                }

                                paged = paged.ToArray();

                                response.AddValue(name + "_total", extent.Build(this.transaction, this.pull.Arguments).Count.ToString());
                                response.AddCollection(name, paged, include);
                            }
                            else
                            {
                                response.AddCollection(name, objects, include);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Extent: {extent.ObjectType}, {result}", e);
                    }
                }
            }
            else
            {
                var name = extent.ObjectType.PluralName;
                response.AddCollection(name, objects);
            }
        }
    }
}
