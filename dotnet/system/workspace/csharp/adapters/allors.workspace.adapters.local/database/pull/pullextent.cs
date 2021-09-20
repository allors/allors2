// <copyright file="LocalPullExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>

namespace Allors.Workspace.Adapters.Local
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using Database.Data;
    using Database.Security;
    using Database.Services;
    using Extent = Database.Extent;

    public class PullExtent
    {
        private readonly IAccessControl acls;
        private readonly IPreparedExtents preparedExtents;
        private readonly IPreparedSelects preparedSelects;
        private readonly Database.Data.Pull pull;
        private readonly ITransaction transaction;

        public PullExtent(ITransaction transaction, Database.Data.Pull pull, IAccessControl acls,
            IPreparedSelects preparedSelects,
            IPreparedExtents preparedExtents)
        {
            this.transaction = transaction;
            this.pull = pull;
            this.acls = acls;
            this.preparedExtents = preparedExtents;
            this.preparedSelects = preparedSelects;
        }

        public void Execute(Pull response)
        {
            var extent = this.pull.Extent ?? (this.pull.ExtentRef.HasValue ? this.preparedExtents.Get(this.pull.ExtentRef.Value) : null);

            if (extent == null)
            {
                throw new Exception("Either an Extent or an ExtentRef is required.");
            }

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
                            var include = select.Include ?? select.End.Include;

                            if (select.PropertyType != null)
                            {
                                objects = select.IsOne
                                    ? objects.Select(v => select.Get(v, this.acls))
                                        .Where(v => v != null)
                                        .Cast<IObject>()
                                        .Distinct()
                                        .ToArray()
                                    : objects
                                        .SelectMany(v =>
                                        {
                                            var stepResult = select.Get(v, this.acls);
                                            return stepResult is HashSet<object> set
                                                ? set.Cast<IObject>().ToArray()
                                                : ((Extent)stepResult)?.ToArray() ?? Array.Empty<IObject>();
                                        })
                                        .Distinct()
                                        .ToArray();

                                var propertyType = select.End.PropertyType;
                                name ??= propertyType.PluralFullName;
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
