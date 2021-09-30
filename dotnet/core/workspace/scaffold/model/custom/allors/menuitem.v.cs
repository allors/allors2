// <copyright file="MenuItem.v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
