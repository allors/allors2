// <copyright file="ObjectExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Concurrent;
    using System.Linq;

    using Allors.Meta;

    public static partial class ObjectExtensions
    {
        private static readonly ConcurrentDictionary<string, RoleType[]> RequiredRoleTypesByClassName = new ConcurrentDictionary<string, RoleType[]>();
        private static readonly ConcurrentDictionary<string, RoleType[]> UniqueRoleTypesByClassName = new ConcurrentDictionary<string, RoleType[]>();

        public static void CoreOnPreDerive(this Object @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            if (derivation.IsModified(@this))
            {
                derivation.Add(@this);
            }
        }

        public static void CoreOnPostDerive(this Object @this, ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;
            var @class = (Class)@this.Strategy.Class;

            // Required
            if (!RequiredRoleTypesByClassName.TryGetValue(@class.Name, out var requiredRoleTypes))
            {
                requiredRoleTypes = @class.ConcreteRoleTypes
                    .Where(concreteRoleType => concreteRoleType.IsRequired)
                    .Select(concreteRoleType => concreteRoleType.RoleType)
                    .ToArray();

                RequiredRoleTypesByClassName[@class.Name] = requiredRoleTypes;
            }

            foreach (var roleType in requiredRoleTypes)
            {
                derivation.Validation.AssertExists(@this, roleType);
            }

            // Unique
            if (!UniqueRoleTypesByClassName.TryGetValue(@class.Name, out var uniqueRoleTypes))
            {
                uniqueRoleTypes = @class.ConcreteRoleTypes
                    .Where(concreteRoleType => concreteRoleType.IsUnique)
                    .Select(concreteRoleType => concreteRoleType.RoleType)
                    .ToArray();

                UniqueRoleTypesByClassName[@class.Name] = uniqueRoleTypes;
            }

            foreach (var roleType in uniqueRoleTypes)
            {
                derivation.Validation.AssertIsUnique(@this, roleType);
            }
        }
    }
}
