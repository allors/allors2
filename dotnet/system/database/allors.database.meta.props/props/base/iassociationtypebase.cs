// <copyright file="IAssociationTypeBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    public partial interface IAssociationTypeBase : IPropertyTypeBase, IAssociationType
    {
        new ICompositeBase ObjectType { get; set; }

        new IRelationTypeBase RelationType { get; }

        new IRoleTypeBase RoleType { get; }

        void Validate(ValidationLog validationLog);
    }
}
