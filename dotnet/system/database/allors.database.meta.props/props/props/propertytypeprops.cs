// <copyright file="AssociationTypeProps.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Database.Meta
{
    public abstract partial class PropertyTypeProps : OperandTypeProps
    {
        public IObjectType ObjectType => this.AsPropertyType.ObjectType;

        public string Name => this.AsPropertyType.Name;

        public string SingularName => this.AsPropertyType.SingularName;

        public string PluralName => this.AsPropertyType.PluralName;

        public bool IsOne => this.AsPropertyType.IsOne;

        public bool IsMany => this.AsPropertyType.IsMany;

        #region As
        protected abstract IPropertyTypeBase AsPropertyType { get; }
        #endregion
    }
}
