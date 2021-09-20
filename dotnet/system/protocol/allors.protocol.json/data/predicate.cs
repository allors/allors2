// <copyright file="Predicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Data
{
    public class Predicate : IVisitable
    {
        /// <summary>
        /// Kind
        /// </summary>
        public PredicateKind k { get; set; }

        /// <summary>
        /// Association Type Tag
        /// </summary>
        public string a { get; set; }

        /// <summary>
        /// Role Type Tag
        /// </summary>
        public string r { get; set; }

        /// <summary>
        /// Object Type Tag
        /// </summary>
        public string o { get; set; }

        /// <summary>
        /// Parameter
        /// </summary>
        public string p { get; set; }

        /// <summary>
        /// Dependencies
        /// </summary>
        public string[] d { get; set; }

        /// <summary>
        /// Operand
        /// </summary>
        public Predicate op { get; set; }

        /// <summary>
        /// Operands
        /// </summary>
        public Predicate[] ops { get; set; }

        /// <summary>
        /// Object
        /// </summary>
        public long? ob { get; set; }

        /// <summary>
        /// Objects
        /// </summary>
        public long[] obs { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public object v { get; set; }

        /// <summary>
        /// Values
        /// </summary>
        public object[] vs { get; set; }

        /// <summary>
        /// Path Role Type Tag
        /// </summary>
        public string pa { get; set; }

        /// <summary>
        /// Path Role Type Tags
        /// </summary>
        public string[] pas { get; set; }

        /// <summary>
        /// Extent
        /// </summary>
        public Extent e { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitPredicate(this);
    }
}
