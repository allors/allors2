// <copyright file="Project.v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
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