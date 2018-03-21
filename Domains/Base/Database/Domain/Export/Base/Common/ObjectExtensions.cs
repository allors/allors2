// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System.Collections.Concurrent;
    using System.Linq;

    using Allors.Meta;

    public static partial class ObjectExtensions
    {
        private static readonly ConcurrentDictionary<string, RoleType[]> RequiredRoleTypesByClassName = new ConcurrentDictionary<string, RoleType[]>();
        private static readonly ConcurrentDictionary<string, RoleType[]> UniqueRoleTypesByClassName = new ConcurrentDictionary<string, RoleType[]>(); 

        public static void BaseOnPreDerive(this Object @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            if (derivation.IsModified(@this))
            {
                if (!derivation.DerivedObjects.Contains(@this))
                {
                    derivation.AddDerivable(@this);
                }
            }
        }

        public static void BaseOnPostDerive(this Object @this, ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;
            var @class = (Class)@this.Strategy.Class;

            // Required
            RoleType[] requiredRoleTypes;
            if (!RequiredRoleTypesByClassName.TryGetValue(@class.Name, out requiredRoleTypes))
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
            RoleType[] uniqueRoleTypes;
            if (!UniqueRoleTypesByClassName.TryGetValue(@class.Name, out uniqueRoleTypes))
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