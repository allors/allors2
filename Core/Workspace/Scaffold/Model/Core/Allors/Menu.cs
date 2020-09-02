// <copyright file="Menu.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public partial class Menu
    {
        public Model Model { get; set; }

        public MenuItem[] MenuItems { get; set; }

        public void BaseLoad(JArray jsonArray) =>
            this.MenuItems = jsonArray?.Cast<JObject>()
                .Select(v =>
                {
                    var child = new MenuItem
                    {
                        Menu = this,
                    };
                    child.Load(v);
                    return child;
                }).ToArray();
    }
}
