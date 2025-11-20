// <copyright file="IObjectTypeExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>

namespace Allors
{
    using System;
    using Allors.Meta;

    public static class IObjectTypeExtensions
    {
        public static IObjects GetObjects(this IObjectType objectType, ISession session)
        {
            var objectFactory = session.Database.ObjectFactory;
            var type = objectFactory.Assembly.GetType(objectFactory.Namespace + "." + objectType.PluralName);
            return (IObjects)Activator.CreateInstance(type, new object[] { session });
        }
    }
}
