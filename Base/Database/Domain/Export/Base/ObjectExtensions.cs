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

namespace Allors
{
    using System;

    using Allors.Domain;
    using Allors.Meta;

    public static partial class ObjectExtensions
    {
        public static void BaseOnPostBuild(this Domain.Object @this, ObjectOnPostBuild method)
        {
            // TODO: Optimize
            foreach (var concreteRoleType in ((Class)@this.Strategy.Class).ConcreteRoleTypes)
            {
                if (concreteRoleType.IsRequired)
                {
                    var relationType = concreteRoleType.RelationType;
                    var unit = relationType.RoleType.ObjectType as IUnit;
                    if (unit != null && !@this.Strategy.ExistRole(relationType))
                    {
                        switch (unit.UnitTag)
                        {
                            case UnitTags.Boolean:
                                @this.Strategy.SetUnitRole(relationType, false);
                                break;
                            case UnitTags.Decimal:
                                @this.Strategy.SetUnitRole(relationType, 0m);
                                break;
                            case UnitTags.Float:
                                @this.Strategy.SetUnitRole(relationType, 0d);
                                break;
                            case UnitTags.Integer:
                                @this.Strategy.SetUnitRole(relationType, 0);
                                break;
                            case UnitTags.Unique:
                                @this.Strategy.SetUnitRole(relationType, Guid.NewGuid());
                                break;
                            case UnitTags.DateTime:
                                @this.Strategy.SetUnitRole(relationType, @this.Strategy.Session.Now());
                                break;
                        }
                    }
                }
            }
        }

        // TODO: move to security
        public static void AddCreatorSecurityToken(this Domain.Object @this)
        {
            var creator = @this.Strategy.Session.GetUser();

            if (creator != null)
            {
                @this.AddSecurityToken(creator.OwnerSecurityToken);
            }
        }
    }
}