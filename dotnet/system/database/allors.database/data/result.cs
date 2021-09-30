// <copyright file="Result.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System;

    public class Result : IVisitable
    {
        public Guid? SelectRef { get; set; }

        public Select Select { get; set; }

        public Node[] Include { get; set; }

        public string Name { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public override string ToString()
        {
            if (this.SelectRef != null)
            {
                return $"Result: [SelectRef: {this.SelectRef}]";
            }

            if (this.Name != null)
            {
                return $"Result: [Name: {this.Name}]";
            }

            return $"Result: [Select: {this.Select}]";
        }

        public void Accept(IVisitor visitor) => visitor.VisitResult(this);
    }
}
