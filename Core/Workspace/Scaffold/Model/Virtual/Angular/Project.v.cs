// <copyright file="Project.v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Angular
{
    using Newtonsoft.Json.Linq;

    public partial class Project
    {
        public void Load(JObject jsonProject)
        {
            this.BaseLoad(jsonProject);

            this.CustomLoad(jsonProject);
        }
    }
}
