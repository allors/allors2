// <copyright file="ConcreteRoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    public sealed partial class ConcreteRoleType : IConcreteRoleType
    {
        public ConcreteRoleType(Class @class, RoleType roleType)
        {
            this.Class = @class;
            this.RoleType = roleType;
        }

        public bool IsRequired => this.IsRequiredOverride ?? ((RoleType)this.RoleType).IsRequired;

        public bool? IsRequiredOverride { get; set; }

        public bool IsUnique => this.IsUniqueOverride ?? ((RoleType)this.RoleType).IsUnique;

        public bool? IsUniqueOverride { get; set; }

        public IRoleType RoleType { get; set; }

        public IRelationType RelationType => this.RoleType.RelationType;

        public Class Class { get; }

        public static implicit operator RoleType(ConcreteRoleType concreteRoleType) => (RoleType)concreteRoleType.RoleType;
    }
}
