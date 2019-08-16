// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Build.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
