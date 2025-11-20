// <copyright file="IInterface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    using System.Collections.Generic;

    public interface IInterface : IComposite
    {
        IEnumerable<IComposite> Subtypes { get; }

        IEnumerable<IClass> Subclasses { get; }
    }
}
