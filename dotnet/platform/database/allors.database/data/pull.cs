// <copyright file="Pull.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public class Pull
    {
        public Guid? ExtentRef { get; set; }

        public IExtent Extent { get; set; }

        public IObjectType ObjectType { get; set; }

        public IObject Object { get; set; }

        public Result[] Results { get; set; }

        public IDictionary<string, string> Parameters { get; set; }

        public Protocol.Data.Pull Save() =>
            new Protocol.Data.Pull
            {
                extentRef = this.ExtentRef,
                extent = this.Extent?.Save(),
                objectType = this.ObjectType?.Id,
                @object = this.Object?.Id.ToString(),
                results = this.Results?.Select(v => v.Save()).ToArray(),
            };
    }
}
