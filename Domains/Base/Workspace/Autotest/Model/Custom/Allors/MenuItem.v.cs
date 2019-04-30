// <copyright file="MenuItem.v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest
{
    using Newtonsoft.Json.Linq;

    public partial class MenuItem
    {
        public void Load(JObject json)
        {
            this.BaseLoadMenu(json);
            this.CustomLoadMenu(json);
        }
    }
}
