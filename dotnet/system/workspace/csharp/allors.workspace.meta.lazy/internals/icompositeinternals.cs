// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;

    public interface ICompositeInternals : IComposite
    {
        new MetaPopulation MetaPopulation { get; set; }

        new ISet<IInterfaceInternals> DirectSupertypes { get; set; }

        new ISet<IInterfaceInternals> Supertypes { get; set; }

        IRoleTypeInternals[] ExclusiveRoleTypes { get; set; }

        IAssociationTypeInternals[] ExclusiveAssociationTypes { get; set; }

        IMethodTypeInternals[] ExclusiveMethodTypes { get; set; }

        void Bind(Dictionary<string, Type> typeByName);
    }
}
