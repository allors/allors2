// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pull.cs" company="Allors bvba">
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

    using Allors.Protocol.Remote.Pull;

    public class Pull
    {
        private readonly PullExtent[] pullExtents;

        public Pull(ISession session, PullRequest req)
        {
            if (req.E != null)
            {
                this.pullExtents = req.E.Select(v => new PullExtent(session).Merge(v)).ToArray();
                foreach (var pull in pullExtents)
                {
                    //var validation = pull.Validate();
                    //if (validation.HasErrors)
                    //{
                    //    var message = validation.ToString();
                    //    this.Logger.LogError(message);
                    //    return this.StatusCode(400, message);
                    //}
                }
            }
        }

        public void Resolve(PullResponseBuilder response)
        {
            foreach (var pull in pullExtents)
            {
                pull.Resolve(response);
            }
        }
    }
}