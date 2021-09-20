// <copyright file="RoleTypeProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta
{
    public sealed partial class RoleTypeProps : PropertyTypeProps
    {
        private readonly IRoleTypeBase roleType;

        internal RoleTypeProps(IRoleTypeBase relationType) => this.roleType = relationType;

        public IAssociationType AssociationType => this.roleType.AssociationType;

        public IRelationType RelationType => this.roleType.RelationType;

        public string FullName => this.roleType.FullName;
        public bool ExistAssignedSingularName => this.roleType.ExistAssignedSingularName;

        public string SingularFullName => this.roleType.SingularFullName;

        public bool ExistAssignedPluralName => this.roleType.ExistAssignedPluralName;

        public string PluralFullName => this.roleType.PluralFullName;

        public int? Size => this.roleType.Size;

        public int? Precision => this.roleType.Precision;

        public int? Scale => this.roleType.Scale;

        public bool IsRequired => this.roleType.IsRequired;

        public bool IsUnique => this.roleType.IsUnique;

        public string MediaType => this.roleType.MediaType;

        #region As
        protected override IMetaObjectBase AsMetaObject => this.roleType;

        protected override IOperandTypeBase AsOperandType => this.roleType;

        protected override IPropertyTypeBase AsPropertyType => this.roleType;
        #endregion
    }
}
