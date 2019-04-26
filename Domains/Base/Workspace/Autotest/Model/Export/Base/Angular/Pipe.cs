// <copyright file="Pipe.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    using Newtonsoft.Json.Linq;

    public class Pipe
    {
        public Project Project { get; set; }

        public JToken Json { get; set; }

        public Reference Reference { get; set; }

        public void BaseLoad()
        {
        }
    }
}