// <copyright file="Result.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;

    public class Result
    {
        public Guid? FetchRef { get; set; }

        public Fetch Fetch { get; set; }

        public string Name { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public Protocol.Data.Result ToJson() =>
            new Protocol.Data.Result
            {
                fetchRef = this.FetchRef,
                fetch = this.Fetch?.ToJson(),
                name = this.Name,
                skip = this.Skip,
                take = this.Take,
            };
    }
}
