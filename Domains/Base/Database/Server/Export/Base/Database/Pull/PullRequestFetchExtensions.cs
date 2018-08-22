// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullRequestFetchExtensions.cs" company="Allors bvba">
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
    using Allors.Meta;
    using Allors.Protocol.Remote;
    using Allors.Protocol.Remote.Pull;

    public static class PullRequestFetchExtensions
    {
        public static void Parse<T>(this PullRequestFetch @this, ISession session, out T @object, out Path path, out Tree include)
            where T : IObject
        {
            @object = (T)session.Instantiate(@this.Id);

            path = null;
            if (@this.Path != null)
            {
                path = new Path();
                @this.Path.Parse(path, session.Database.MetaPopulation);
            }

            include = null;
            if (@this.Include != null && @object != null)
            {
                if (path == null)
                {
                    var composite = @object.Strategy.Class;
                    include = new Tree(composite);
                }
                else
                {
                    include = new Tree((IComposite)path.End.GetObjectType());
                }

                var metaPopulation = (MetaPopulation)@object.Strategy.Session.Database.MetaPopulation;
                foreach (var i in @this.Include)
                {
                    i.Parse(metaPopulation, out TreeNode treeNode);
                    include.Nodes.Add(treeNode);
                }
            }            
        }
    }
}