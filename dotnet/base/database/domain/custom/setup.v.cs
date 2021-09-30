// <copyright file="Setup.v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    public partial class Setup
    {
        private void OnPrePrepare()
        {
            this.CoreOnPrePrepare();
            this.BaseOnPrePrepare();
            this.CustomOnPrePrepare();
        }

        private void OnPostPrepare()
        {
            this.CoreOnPostPrepare();
            this.BaseOnPostPrepare();
            this.CustomOnPostPrepare();
        }

        private void OnPreSetup()
        {
            this.CoreOnPreSetup();
            this.BaseOnPreSetup();
            this.CustomOnPreSetup();
        }

        private void OnPostSetup()
        {
            this.CoreOnPostSetup();
            this.BaseOnPostSetup();
            this.CustomOnPostSetup();
        }
    }
}
