// <copyright file="Singleton.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    public partial class Singleton : Auditable
    {
        #region inherited properties
        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Allors
        [Id("D53B80EE-468D-463C-8BBF-725105DBA376")]
        [AssociationId("18C9321B-0846-401D-825F-16BAAE18708D")]
        [RoleId("4541B748-349F-4BEF-86F2-3644423FE0AB")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        [Workspace]
        public Settings Settings { get; set; }

        #region Allors
        [Id("d9ea02e5-9aa1-4cbe-9318-06324529a923")]
        [AssociationId("6247e69d-4789-4ee0-a75b-c2de44a5fcce")]
        [RoleId("c11f31e1-75a7-4b23-9d58-7dfec256b658")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SecurityToken AdministratorSecurityToken { get; set; }

        #region Allors
        [Id("3BA81A3E-2F3D-4270-BEF8-CF72CD978355")]
        [AssociationId("61ABB4A7-79AA-4388-8C0D-1369BC862046")]
        [RoleId("6799208F-C7FB-4CBD-B5F7-180C8A212B13")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public UserGroup EmployeeUserGroup { get; set; }

        #region Allors
        [Id("06DC7598-3CF1-43F5-A72A-2D1CE40BEBDA")]
        [AssociationId("BB17AD20-7B26-4202-87B7-4A1F1ADBDE2B")]
        [RoleId("E58AC925-007F-4EE9-8C84-E7E59BB775C2")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        public AccessControl EmployeeAccessControl { get; set; }

        #region inherited methods

        #endregion
    }
}
