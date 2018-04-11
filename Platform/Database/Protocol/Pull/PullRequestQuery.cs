// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullRequestQuery.cs" company="Allors bvba">
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
    public class PullRequestQuery
    {
        /// <summary>
        /// Gets or sets the name of the Query
        /// </summary>
        public string N { get; set; }

        /// <summary>
        /// Gets or sets the ObjectType.
        /// </summary>
        public string OT { get; set; }

        /// <summary>
        /// Gets or sets the predicate
        /// </summary>
        public PullRequestPredicate P { get; set; }

        public PullRequestQuery[] UN { get; set; }

        public PullRequestQuery[] IN { get; set; }

        public PullRequestQuery[] EX { get; set; }
        
        public PullRequestTreeNode[] I { get; set; }

        public PullRequestSort[] S { get; set; }

        public PullRequestPage PA { get; set; }
    }
}