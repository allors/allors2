// <copyright file="IMetaObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    using System;

    /// <summary>
    /// Base interface for Meta objects.
    /// </summary>
    public interface IMetaObject
    {
        Guid Id { get; }

        string IdAsString { get; }

        IMetaPopulation MetaPopulation { get; }
    }
}
