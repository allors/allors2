//-------------------------------------------------------------------------------------------------
// <copyright file="PullInstantiate.cs" company="Allors bvba">
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
    using System.Linq;

    using Allors.Data;
    using Allors.Domain;
    using Allors.Services;

    public class PullInstantiate
    {
        private readonly ISession session;

        private readonly Pull pull;

        private readonly User user;

        private readonly IPathService pathService;

        public PullInstantiate(ISession session, Pull pull, User user, IPathService pathService)
        {
            this.session = session;
            this.pull = pull;
            this.user = user;
            this.pathService = pathService;
        }

        public void Execute(PullResponseBuilder response)
        {
            var @object = this.session.Instantiate(this.pull.Object);

            if (this.pull.Results != null)
            {
                foreach (var result in this.pull.Results)
                {
                    var name = result.Name;

                    var fetch = result.Path;
                    if (fetch == null && result.PathRef.HasValue)
                    {
                        fetch = this.pathService.Get(result.PathRef.Value);
                    }

                    if (fetch != null)
                    {
                        if (fetch.Path != null)
                        {
                            var aclCache = new AccessControlListCache(this.user);

                            var propertyType = fetch.Path.End.PropertyType;

                            if (propertyType.IsOne)
                            {
                                name = name ?? propertyType.SingularName;

                                @object = (IObject)fetch.Path.Get(@object, aclCache);
                                response.AddObject(name, @object, fetch.Include);
                            }
                            else
                            {
                                name = name ?? propertyType.SingularName;

                                var objects = ((Extent)fetch.Path.Get(@object, aclCache)).ToArray();

                                if (result.Skip.HasValue)
                                {
                                    var paged = objects.Skip(result.Skip.Value);
                                    if (result.Take.HasValue)
                                    {
                                        paged = paged.Take(result.Take.Value);
                                    }

                                    paged = paged.ToArray();

                                    response.AddValue(name + "_total", objects.Length);
                                    response.AddCollection(name, paged, fetch.Include);
                                }
                                else
                                {
                                    response.AddCollection(name, objects, fetch.Include);
                                }
                            }
                        }
                        else
                        {
                            name = name ?? @object.Strategy.Class.SingularName;
                            response.AddObject(name, @object, fetch.Include);
                        }
                    }
                    else
                    {
                        name = name ?? @object.Strategy.Class.SingularName;
                        response.AddObject(name, @object);
                    }
                }
            }
            else
            {
                var name = @object.Strategy.Class.SingularName;
                response.AddObject(name, @object);
            }
        }
    }
}