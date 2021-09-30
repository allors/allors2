// <copyright file="LocalPullInstantiate.cs" company="Allors bvba">
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
    using Database.Meta;
    using Database.Security;
    using Extent = Database.Extent;

    public class PullInstantiate
    {
        private readonly IAccessControl acls;
        private readonly IPreparedSelects preparedSelects;
        private readonly Database.Data.Pull pull;
        private readonly ITransaction transaction;

        public PullInstantiate(ITransaction transaction, Database.Data.Pull pull, IAccessControl acls, IPreparedSelects preparedSelects)
        {
            this.transaction = transaction;
            this.pull = pull;
            this.acls = acls;
            this.preparedSelects = preparedSelects;
        }

        public void Execute(Pull response)
        {
            var @object = this.transaction.Instantiate(this.pull.Object);

            var @class = @object.Strategy?.Class;

            if (@class != null && this.pull.ObjectType is IComposite objectType && !objectType.IsAssignableFrom(@class))
            {
                return;
            }

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
                                var propertyType = select.End.PropertyType;

                                if (select.IsOne)
                                {
                                    name ??= propertyType.SingularFullName;

                                    @object = (IObject)select.Get(@object, this.acls);
                                    response.AddObject(name, @object, include);
                                }
                                else
                                {
                                    name ??= propertyType.PluralFullName;

                                    var stepResult = select.Get(@object, this.acls);
                                    var objects = stepResult is HashSet<object> set
                                        ? set.Cast<IObject>().ToArray()
                                        : ((Extent)stepResult)?.ToArray() ?? Array.Empty<IObject>();

                                    if (result.Skip.HasValue || result.Take.HasValue)
                                    {
                                        var paged = result.Skip.HasValue ? objects.Skip(result.Skip.Value) : objects;
                                        if (result.Take.HasValue)
                                        {
                                            paged = paged.Take(result.Take.Value);
                                        }

                                        paged = paged.ToArray();

                                        response.AddValue(name + "_total", objects.Length.ToString());
                                        response.AddCollection(name, paged, include);
                                    }
                                    else
                                    {
                                        response.AddCollection(name, objects, include);
                                    }
                                }
                            }
                            else
                            {
                                name ??= this.pull.ObjectType?.Name ?? @object.Strategy.Class.SingularName;
                                response.AddObject(name, @object, include);
                            }
                        }
                        else
                        {
                            name ??= this.pull.ObjectType?.Name ?? @object.Strategy.Class.SingularName;
                            var include = result.Include;
                            response.AddObject(name, @object, include);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Instantiate: {@object?.Strategy.Class}[{@object?.Strategy.ObjectId}], {result}", e);
                    }
                }
            }
            else
            {
                var name = this.pull.ObjectType?.Name ?? @object.Strategy.Class.SingularName;
                response.AddObject(name, @object);
            }
        }
    }
}
