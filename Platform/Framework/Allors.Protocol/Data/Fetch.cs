
// <copyright file="Fetch.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    public class Fetch
    {
        public Step Step { get; set; }

        public Tree Include { get; set; }
    }
}
