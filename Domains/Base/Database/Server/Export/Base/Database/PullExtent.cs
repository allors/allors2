//-------------------------------------------------------------------------------------------------
// <copyright file="PullExtent.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Data;
    using Allors.Domain;
    using Allors.Services;

    public class PullExtent
    {
        private readonly ISession session;
        private readonly Pull pull;
        private readonly User user;
        private readonly IExtentService extentService;
        private readonly IFetchService fetchService;

        public PullExtent(
            ISession session,
            Pull pull,
            User user,
            IExtentService extentService,
            IFetchService fetchService)
        {
            this.session = session;
            this.pull = pull;
            this.user = user;
            this.extentService = extentService;
            this.fetchService = fetchService;
        }

        public void Execute(PullResponseBuilder response)
        {
            if (this.pull.Extent == null && !this.pull.ExtentRef.HasValue)
            {
                throw new Exception("Either an Extent or an ExtentRef is required.");
            }

            var extent = this.pull.Extent ?? this.extentService.Get(this.pull.ExtentRef.Value);
            var objects = extent.Build(this.session, this.pull.Arguments).ToArray();

            if (this.pull.Results != null)
            {
                foreach (var result in this.pull.Results)
                {
                    var name = result.Name;

                    var fetch = result.Fetch;
                    if (fetch != null)
                    {
                        if (fetch.Path != null)
                        {
                            var aclCache = new AccessControlListCache(this.user);

                            var propertyType = fetch.Path.End.PropertyType;

                            if (propertyType.IsOne)
                            {
                                objects = objects.Select(v => fetch.Path.Get(v, aclCache)).Where(v => v != null).Cast<IObject>()
                                    .Distinct().ToArray();
                            }
                            else
                            {
                                objects = objects.SelectMany(v => ((Extent)fetch.Path.Get(v, aclCache)).ToArray()).Distinct()
                                    .ToArray();
                            }

                            name = name ?? propertyType.PluralName;
                        }

                        name = name ?? this.pull.DefaultResultName(fetch);

                        if (result.Skip.HasValue)
                        {
                            var paged = objects.Skip(result.Skip.Value);
                            if (result.Take.HasValue)
                            {
                                paged = paged.Take(result.Take.Value);
                            }

                            paged = paged.ToArray();

                            response.AddValue(name + "_total", extent.Build(this.session, this.pull.Arguments).Count);
                            response.AddCollection(name, paged, fetch.Include);
                        }
                        else
                        {
                            response.AddCollection(name, objects, fetch.Include);
                        }
                    }
                    else
                    {
                        name = name ?? this.pull.DefaultResultName();
                        response.AddCollection(name, objects);
                    }
                }
            }
            else
            {
                var name = this.pull.DefaultResultName();
                response.AddCollection(name, objects);
            }
        }
    }
}