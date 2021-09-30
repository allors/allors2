// <copyright file="Provider.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Angular
{
    using Newtonsoft.Json.Linq;

    public partial class Provider
    {
        public Project Project { get; set; }

        public JToken Json { get; set; }

        public Reference TokenIdentifier { get; set; }

        public string TokenValue { get; set; }

        public Reference UseClass { get; set; }

        public JToken UseExisting { get; set; }

        public Reference UseFactory { get; set; }

        public bool Multi { get; set; }

        public void BaseLoad()
        {
        }
    }
}
