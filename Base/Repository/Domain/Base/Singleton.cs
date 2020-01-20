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
        [Id("076E1D78-8C6A-4A9D-A023-106D3EFB3B87")]
        [AssociationId("7F5BC6FF-92F1-444A-865B-D74DE39E0581")]
        [RoleId("93A2BD2B-A8BC-41C0-8241-0C81FB5EFF6D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public Template NonUnifiedPartBarcodePrintTemplate { get; set; }

        #region Allors
        [Id("CDB21C6E-CEE4-4E2B-839E-CA2F414B4EF9")]
        [AssociationId("27A8200A-3688-4F3F-AB01-6A5029FAF2A2")]
        [RoleId("32E58B93-F8E9-43DA-A081-29C87A48FCEF")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        public NonUnifiedPartBarcodePrint NonUnifiedPartBarcodePrint { get; set; }

        #region inherited methods

        #endregion
    }
}
