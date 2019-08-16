//-------------------------------------------------------------------------------------------------
// <copyright file="Extent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Protocol.Data
{
    using System;

    public class Extent
    {
        public string Kind { get; set; }

        public Extent[] Operands { get; set; }

        public Guid? ObjectType { get; set; }

        public Predicate Predicate { get; set; }

        public Sort[] Sorting { get; set; }
    }
}
