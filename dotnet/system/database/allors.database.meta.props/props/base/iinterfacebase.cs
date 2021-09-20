// <copyright file="IClassBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    using System.Collections.Generic;

    public partial interface IInterfaceBase : ICompositeBase, IInterface
    {
        new IEnumerable<ICompositeBase> Subtypes { get; }

        void DeriveDirectSubtypes(HashSet<ICompositeBase> sharedCompositeTypes);

        void DeriveSubtypes(HashSet<ICompositeBase> sharedCompositeTypes);

        void DeriveSubclasses(HashSet<IClassBase> sharedClasses);

        void DeriveExclusiveSubclass();

        void DeriveWorkspaceNames();

        void DeriveSupertypesRecursively(IObjectTypeBase type, HashSet<IInterfaceBase> superTypes);
    }
}
