// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectBuilder.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
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