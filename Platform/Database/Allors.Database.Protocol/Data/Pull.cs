// <copyright file="Pull.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;
    using System.Collections.Generic;

    public class Pull
    {
        public Guid? ExtentRef { get; set; }

        public Extent Extent { get; set; }

        public Guid? ObjectType { get; set; }

        public string Object { get; set; }

        public Result[] Results { get; set; }

        public IDictionary<string, object> Arguments { get; set; }
    }
}
