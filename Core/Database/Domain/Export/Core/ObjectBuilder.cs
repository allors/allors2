// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;

    public abstract class ObjectBuilder : IObjectBuilder
    {
        public static readonly Dictionary<IObjectType, Type> BuilderTypeByObjectTypeId = new Dictionary<IObjectType, Type>();

        public static object Build(ISession session, IClass @class)
        {
            Type builderType;
            if (!BuilderTypeByObjectTypeId.TryGetValue(@class, out builderType))
            {
                var builderTypeName = "Allors.Domain." + @class.Name + "Builder";
                builderType = Type.GetType(builderTypeName, false);
                if (builderType != null)
                {
                    BuilderTypeByObjectTypeId[@class] = builderType;
                }
            }

            if (builderType != null)
            {
                object[] parameters = { session };
                var builder = (IObjectBuilder)Activator.CreateInstance(builderType, parameters);
                return builder.DefaultBuild();
            }

            return session.Create(@class);
        }

        public abstract void Dispose();

        public abstract IObject DefaultBuild();
    }
}
