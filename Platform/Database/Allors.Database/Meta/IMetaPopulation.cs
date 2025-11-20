// <copyright file="IMetaPopulation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMetaPopulation
    {
        IEnumerable<IUnit> Units { get; }

        IEnumerable<IComposite> Composites { get; }

        IEnumerable<IClass> Classes { get; }

        IEnumerable<IInterface> Interfaces { get; }

        IEnumerable<IRelationType> RelationTypes { get; }

        IMetaObject Find(Guid metaObjectId);

        IClass FindClassByName(string name);

        bool IsValid { get; }

        IValidationLog Validate();

        void Bind(Type[] types, MethodInfo[] methods);
    }
}
