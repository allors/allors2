// <copyright file="IAssociationTypeBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    public partial interface IRoleTypeBase : IPropertyTypeBase, IRoleType
    {
        new IObjectTypeBase ObjectType { get; set; }

        new IAssociationTypeBase AssociationType { get; }

        new IRelationTypeBase RelationType { get; }

        bool ExistAssignedSingularName { get; }

        new string SingularName { get; set; }

        bool ExistAssignedPluralName { get; }

        new string PluralName { get; set; }

        new int? Size { get; set; }

        new int? Precision { get; set; }

        new int? Scale { get; set; }

        void DeriveScaleAndSize();

        void Validate(ValidationLog validationLog);
    }
}
