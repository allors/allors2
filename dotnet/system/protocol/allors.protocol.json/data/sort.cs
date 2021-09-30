// <copyright file="Sort.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Data
{
    public class Sort : IVisitable
    {
        /// <summary>
        /// Role Type Tag
        /// </summary>
        public string r { get; set; }

        /// <summary>
        /// Sort Direction
        /// </summary>
        public SortDirection d { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitSort(this);
    }
}
