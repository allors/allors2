// <copyright file="Address.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("FA760DB7-59FE-49C7-8198-6A08A2DFDEF9")]
    #endregion
    public partial interface Addressable : Object
    {
        #region Allors
        [Id("5E8CA38B-F4FB-4BBF-8A60-4DDA8EA6EF0E")]
        [AssociationId("4376D368-4550-4D2A-9E3C-9BDAC4C72688")]
        [RoleId("90C711EA-0719-46C6-9FB9-FC6E7A5B91C5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        Address Address { get; set; }
    }
}
