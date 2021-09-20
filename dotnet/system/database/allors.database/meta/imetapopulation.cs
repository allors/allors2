// <copyright file="IMetaPopulation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMetaPopulation
    {
        IEnumerable<IDomain> Domains { get; }

        IEnumerable<IUnit> Units { get; }

        IEnumerable<IComposite> Composites { get; }

        IEnumerable<IInterface> Interfaces { get; }

        IEnumerable<IClass> Classes { get; }

        IEnumerable<IRelationType> RelationTypes { get; }

        IEnumerable<IMethodType> MethodTypes { get; }

        IEnumerable<IInheritance> Inheritances { get; }

        IEnumerable<IComposite> DatabaseComposites { get; }

        IEnumerable<IInterface> DatabaseInterfaces { get; }

        IEnumerable<IClass> DatabaseClasses { get; }

        IEnumerable<IRelationType> DatabaseRelationTypes { get; }

        IEnumerable<string> WorkspaceNames { get; }

        bool IsValid { get; }

        IMetaIdentifiableObject FindById(Guid metaObjectId);

        IMetaIdentifiableObject FindByTag(string tag);

        IObjectType FindDatabaseCompositeByName(string name);

        IValidationLog Validate();

        void Bind(Type[] types, Dictionary<Type, MethodInfo[]> extensionMethodsByInterface);
    }
}
