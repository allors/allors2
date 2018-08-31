// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullRequestQueryExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Data;
    using Allors.Domain.Query;
    using Allors.Meta;
    using Allors.Server.Protocol.Pull;

    public static class PullRequestQueryExtensions
    {
        public static Query Parse(this PullRequestQuery @this, MetaPopulation metaPopulation)
        {
            var composite = (Composite)metaPopulation.Find(new Guid(@this.OT));
            var predicate = @this.P?.Parse(metaPopulation);
            
            Tree include = null;
            if (@this.I != null)
            {
                include = new Tree(composite);
                foreach (var i in @this.I)
                {
                    i.Parse(metaPopulation, out TreeNode treeNode);
                    include.Nodes.Add(treeNode);
                }
            }

            var union = @this.UN?.Select(v => v.Parse(metaPopulation)).ToArray();

            var intersect = @this.IN?.Select(v => v.Parse(metaPopulation)).ToArray();

            var except = @this.EX?.Select(v => v.Parse(metaPopulation)).ToArray();

            var sort = @this.S?.Select(v => v.Parse(metaPopulation)).ToArray();

            var page = @this.PA?.Parse();

            var query = new Query
                            {
                                Name = @this.N,
                                ObjectType = composite,
                                Predicate = predicate,
                                Union = union,
                                Intersect = intersect,
                                Except = except,
                                Include = include,
                                Sort = sort,
                                Page = page
                            };

            return query;
        }
    }
}