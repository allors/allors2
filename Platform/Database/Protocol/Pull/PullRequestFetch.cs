// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullRequestFetch.cs" company="Allors bvba">
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
    public class PullRequestFetch
    {
        /// <summary>
        /// Gets or sets the name of the fetch
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the id of the object
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the path
        /// </summary>
        public PullRequestPath Path { get; set; }

        /// <summary>
        /// Gets or sets the additional objects to include from this relations.
        /// </summary>
        public PullRequestTreeNode[] Include { get; set; }
    }
}