// <copyright file="ObjectFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Workspace
{
    using System;
    using Meta;

    public interface IObjectFactory
    {
        IObject Create(IStrategy strategy);

        IObjectType GetObjectType<T>();

        IObjectType GetObjectType(Type type);

        IObjectType GetObjectType(string name);

        Type GetType(IObjectType objectType);
    }
}
