// <copyright file="IExtentOperator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    public interface IExtentOperator : IExtent
    {
        IExtent[] Operands { get; set; }
    }
}
