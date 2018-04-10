// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullRequestPredicate.cs" company="Allors bvba">
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
    public class PullRequestPredicate
    {
        public string _T { get; set; }

        public PullRequestPredicate P { get; set; }

        public PullRequestPredicate[] PS { get; set; }

        public PullRequestQuery Q { get; set; }

        public string OT { get; set; }

        public string AT { get; set; }

        public string RT { get; set; }

        public object V { get; set; }

        public object F { get; set; }

        public object S { get; set; }

        public string O { get; set; }

        public string[] OS { get; set; }
    }
}