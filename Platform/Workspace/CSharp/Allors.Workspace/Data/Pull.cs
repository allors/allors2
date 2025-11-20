// <copyright file="Pull.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    public class Pull
    {
        public Guid? ExtentRef { get; set; }

        public IExtent Extent { get; set; }

        public IObjectType ObjectType { get; set; }

        public ISessionObject Object { get; set; }

        public string ObjectId { get; set; }

        public Arguments Arguments { get; set; }

        public Result[] Results { get; set; }

        public Protocol.Data.Pull ToJson() =>
            new Protocol.Data.Pull
            {
                extentRef = this.ExtentRef,
                extent = this.Extent?.ToJson(),
                objectType = this.ObjectType?.Id,
                @object = this.ObjectId ?? this.Object?.Id.ToString(),
                results = this.Results?.Select(v => v.ToJson()).ToArray(),
            };
    }
}
