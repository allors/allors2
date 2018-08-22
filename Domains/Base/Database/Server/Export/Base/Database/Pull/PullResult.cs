// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullResult.cs" company="Allors bvba">
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
    using System.Linq;

    using Allors.Meta;
    using Allors.Protocol.Remote.Pull;

    public class PullResult
    {
        private readonly string name;

        private readonly PullPage page;

        private readonly Path path;

        private readonly Tree include;

        private readonly IObject[] objects;

        public PullResult(IObject[] objects, string name)
        {
            this.objects = objects;
            this.name = name;
        }

        public PullResult(IObject[] objects, string name, Protocol.Remote.Pull.PullResult result, IComposite composite)
        : this(objects, name)
        {
            this.page = result.Page;

            if (result.Path != null)
            {
                this.path = new Path();
                result.Path.Parse(this.path, composite.MetaPopulation);
            }

            if (result.Include != null)
            {
                this.include = this.path == null ?
                                   new Tree(composite) :
                                   new Tree((IComposite)this.path.End.GetObjectType());

                foreach (var i in result.Include)
                {
                    i.Parse(composite.MetaPopulation, out TreeNode treeNode);
                    this.include.Nodes.Add(treeNode);
                }
            }
        }

        public void Resolve(PullExtent pullExtent, PullResponseBuilder response)
        {
            if (this.page != null)
            {
                var paged = this.objects;
                if (this.page.S.HasValue)
                {
                    paged = paged.Skip(this.page.S.Value).ToArray();
                }

                if (this.page.T.HasValue)
                {
                    paged = paged.Take(this.page.T.Value).ToArray();
                }

                response.AddValue(this.name + "_total", this.objects.Length);
                response.AddCollection(this.name, paged, this.include);
            }
            else
            {
                response.AddCollection(this.name, this.objects.ToArray(), this.include);
            }
        }
    }
}