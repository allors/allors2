//------------------------------------------------------------------------------------------------- 
// <copyright file="Pull.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
//-------------------------------------------------------------------------------------------------

using Allors.Workspace.Meta;

namespace Allors.Workspace.Data
{
    using System;
    using System.Linq;
    using Allors.Workspace;

    public class Pull
    {
        public Guid? ExtentRef { get; set; }

        public IExtent Extent { get; set; }

        public IObjectType ObjectType { get; set; }

        public SessionObject Object { get; set; }

        public Arguments Arguments { get; set; }

        public Result[] Results { get; set; }
        
        public Protocol.Data.Pull ToJson()
        {
            return new Protocol.Data.Pull
            {
                ExtentRef = this.ExtentRef,
                Extent = this.Extent?.ToJson(),
                ObjectType = this.ObjectType?.Id,
                Object = this.Object?.Id.ToString(),
                Results = this.Results?.Select(v => v.ToJson()).ToArray()
            };
        }
    }
}