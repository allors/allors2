// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectBuilder.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

        public static object Build(ISession session, IObjectType objectType)
        {
            if (objectType.IsUnit)
            {
                var unit = (Unit)objectType;
                var unitTypeTag = unit.UnitTag;
                switch (unitTypeTag)
                {
                    case UnitTags.String:
                        return string.Empty;

                    case UnitTags.Integer:
                        return 0;

                    case UnitTags.Decimal:
                        return 0m;

                    case UnitTags.Float:
                        return 0d;

                    case UnitTags.Boolean:
                        return false;

                    case UnitTags.Binary:
                        return new byte[0];

                    case UnitTags.Unique:
                        return Guid.NewGuid();

                    default:
                        throw new ArgumentException("Unknown Unit ObjectType: " + unitTypeTag);
                }
            }

            Type builderType;
            if (!BuilderTypeByObjectTypeId.TryGetValue(objectType, out builderType))
            {
                var builderTypeName = "Allors.Domain." + objectType.Name + "Builder";
                builderType = Type.GetType(builderTypeName, false);
                if (builderType != null)
                {
                    BuilderTypeByObjectTypeId[objectType] = builderType;
                }
            }

            if (builderType != null)
            {
                object[] parameters = { session };
                var builder = (IObjectBuilder)Activator.CreateInstance(builderType, parameters);
                return builder.DefaultBuild();
            }

            return session.Create((Class)objectType);
        }

        public abstract void Dispose();

        public abstract IObject DefaultBuild();
    }
}