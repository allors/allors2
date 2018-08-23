// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullExtent.cs" company="Allors bvba">
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

    public class PullExtent
    {
        private PullResult[] results;

        public PullExtent(ISession session)
        {
            this.Session = session;
        }

        public ISession Session { get; }

        public PullExtent Merge(Protocol.Remote.Pull.PullExtent pullExtent)
        {
            var extent = pullExtent.Extent.Load(this.Session).Build(this.Session);
            var composite = extent.ObjectType;
            var objects = extent?.ToArray();

            this.results = pullExtent.Results?.Select(v => new PullResult(objects, v.Name ?? composite.PluralName, v, composite)).ToArray()
                           ?? new[] { new PullResult(objects, composite.PluralName), };

            return this;
        }

        public void Resolve(PullResponseBuilder response)
        {
            foreach (var result in this.results)
            {
                result.Resolve(this, response);
            }
        }
    }
}