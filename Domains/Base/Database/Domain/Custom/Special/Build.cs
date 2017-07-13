// --------------------------------------------------------------------------------------------------------------------
// <copyright file="One.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

namespace Allors.Domain
{
    using System;

    /// <summary>
    /// Shared
    /// </summary>
    public partial class Build
    {
        public void CustomOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistGuid)
            {
                this.Guid = new Guid("DCE649A4-7CF6-48FA-93E4-CDE222DA2A94");
            }
        }

        public void CustomOnPostBuild(ObjectOnPostBuild method)
        {
            if (!this.ExistString)
            {
                this.String = "Exist";
            }
        }

    }
}
