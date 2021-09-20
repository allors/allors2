// <copyright file="IRelationTypeBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    public partial interface IRelationTypeBase : IMetaIdentifiableObjectBase, IRelationType
    {
        string Name { get; }

        new IAssociationTypeBase AssociationType { get; }

        new IRoleTypeBase RoleType { get; }

        void DeriveMultiplicity();

        void DeriveWorkspaceNames();

        void Validate(ValidationLog log);
    }
}
