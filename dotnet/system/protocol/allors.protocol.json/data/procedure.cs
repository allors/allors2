// <copyright file="Pull.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Data
{
    using System.Collections.Generic;

    public class Procedure : IVisitable
    {
        /// <summary>
        /// Name
        /// </summary>
        public string n { get; set; }

        /// <summary>
        /// Collections
        /// </summary>
        public IDictionary<string, long[]> c { get; set; }

        /// <summary>
        /// Objects
        /// </summary>
        public IDictionary<string, long> o { get; set; }

        /// <summary>
        /// Values
        /// </summary>
        public IDictionary<string, string> v { get; set; }

        /// <summary>
        /// Pool
        /// [][id,version]
        /// </summary>
        public long[][] p { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitProcedure(this);
    }
}
