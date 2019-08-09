// <copyright file="Menu.v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest
{
    using Newtonsoft.Json.Linq;

    public partial class Menu
    {
        public void Load(JArray json)
        {
            this.BaseLoad(json);
            this.CustomLoad(json);
        }
    }
}