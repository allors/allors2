// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncResponseObject.cs" company="Allors bvba">
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

namespace Allors.Server.Protocol.Sync
{
    public class SyncResponseObject
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string I { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string V { get; set; }

        /// <summary>
        /// Gets or sets the object type.
        /// </summary>
        public string T { get; set; }

        public object[][] Roles { get; set; }

        public string[][] Methods { get; set; }

        public override string ToString()
        {
            return $"{this.T} [{this.I}:{this.V}]";
        }
    }
}