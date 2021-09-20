// <copyright file="Result.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System;

    public class Result
    {
        public Guid? FetchRef { get; set; }

        public Fetch Fetch { get; set; }

        public string Name { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public Protocol.Data.Result Save() =>
            new Protocol.Data.Result
            {
                fetchRef = this.FetchRef,
                fetch = this.Fetch?.Save(),
                name = this.Name,
                skip = this.Skip,
                take = this.Take,
            };

        public override string ToString()
        {
            if (this.FetchRef != null)
            {
                return $"Result: [FetchRef: {this.FetchRef}]";
            }

            if (this.Name != null)
            {
                return $"Result: [Name: {this.Name}]";
            }

            return $"Result: [Fetch: {this.Fetch}]";
        }
    }
}
