// <copyright file="AssociationTypeProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Database.Meta
{
    public sealed partial class AssociationTypeProps : PropertyTypeProps
    {
        private readonly IAssociationTypeBase associationType;

        internal AssociationTypeProps(IAssociationTypeBase relationType) => this.associationType = relationType;

        public IRelationType RelationType => this.associationType.RelationType;

        public IRoleType RoleType => this.associationType.RoleType;

        public string SingularFullName => this.associationType.SingularFullName;

        public string PluralFullName => this.associationType.PluralFullName;

        #region As
        protected override IMetaObjectBase AsMetaObject => this.associationType;

        protected override IOperandTypeBase AsOperandType => this.associationType;

        protected override IPropertyTypeBase AsPropertyType => this.associationType;
        #endregion
    }
}
