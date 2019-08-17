// <copyright file="ISandbox.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("7ba2ab26-491b-49eb-944c-26f6bb66e50f")]
    #endregion
    public partial interface ISandbox : Object
    {

        #region Allors
        [Id("38361bff-62b3-4607-8291-cfdaeedbd36d")]
        [AssociationId("f5403207-14c6-422e-9139-92e1c46ea15b")]
        [RoleId("675e80d6-5718-4a84-aef0-92ccf07dcdc7")]
        [Size(256)]
        #endregion
        string InvisibleValue { get; set; }

        #region Allors
        [Id("796ab057-88a0-4d71-bc4a-2673a209161b")]
        [AssociationId("34a3ba9b-6ba6-4cbd-977b-bb22b0ea7c10")]
        [RoleId("26fa08b3-598d-4985-9021-02c422fa4494")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        ISandbox[] InvisibleManies { get; set; }

        #region Allors
        [Id("dba5deb2-880d-47f4-adae-0b3125ff1379")]
        [AssociationId("8ad9a7aa-095e-43d9-aa4e-f21f7c70fdbb")]
        [RoleId("3e8d7881-8112-4001-a518-3fcef1a24615")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        ISandbox InvisibleOne { get; set; }

    }
}
