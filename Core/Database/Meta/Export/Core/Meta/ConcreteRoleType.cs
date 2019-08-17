// <copyright file="ConcreteRoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Meta
{
    public sealed partial class ConcreteRoleType : IConcreteRoleType
    {
        public ConcreteRoleType(Class @class, RoleType roleType)
        {
            this.Class = @class;
            this.RoleType = roleType;
        }

        public bool IsRequired => this.IsRequiredOverride ?? this.RoleType.IsRequired;

        public bool? IsRequiredOverride { get; set; }

        public bool IsUnique => this.IsUniqueOverride ?? this.RoleType.IsUnique;

        public bool? IsUniqueOverride { get; set; }

        IRoleType IConcreteRoleType.RoleType
        {
            get => this.RoleType;
            set => this.RoleType = (RoleType)value;
        }

        public RoleType RoleType { get; private set; }

        public RelationType RelationType => this.RoleType.RelationType;

        public Class Class { get; }

        public static implicit operator RoleType(ConcreteRoleType concreteRoleType) => concreteRoleType.RoleType;
    }
}
