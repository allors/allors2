// <copyright file="InterfaceWithoutConcreteClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("2f4bc713-47c9-4e07-9f2b-1d22a0cb4fad")]
    #endregion
    public partial interface InterfaceWithoutConcreteClass : Object
    {

        #region Allors
        [Id("b490715d-e318-471b-bd37-1c1e12c0314e")]
        [AssociationId("6730e78c-e678-4763-aa98-a5de1be1500c")]
        [RoleId("e7edc290-a280-40dc-acc6-a6b7ebbb09b0")]
        #endregion
        bool AllorsBoolean { get; set; }

    }
}
