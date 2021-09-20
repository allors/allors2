// <copyright file="Inheritance.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Inheritance type.</summary>

namespace Allors.Workspace.Meta
{
    public sealed class Inheritance
    {
        public Inheritance(ICompositeInternals subtype, IInterfaceInternals supertype)
        {
            this.Subtype = subtype;
            this.Supertype = supertype;
        }

        public IInterfaceInternals Supertype { get; }

        public ICompositeInternals Subtype { get; }
    }
}
