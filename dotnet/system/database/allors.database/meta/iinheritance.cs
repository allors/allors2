// <copyright file="IInterface.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    public interface IInheritance
    {
        IInterface Supertype { get; }

        IComposite Subtype { get; }
    }
}
