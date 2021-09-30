// <copyright file="Address.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("130aa2ff-4f14-4ad7-8a27-f80e8aebfa00")]
    #endregion
    [Plural("Addresses")]
    public partial interface Address : Object
    {
        #region Allors
        [Id("36e7d935-a9c7-484d-8551-9bdc5bdeab68")]
        [AssociationId("113a8abd-e587-45a3-b118-92e60182c94b")]
        [RoleId("4f7016f6-1b87-4ac4-8363-7f8210108928")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        Place Place { get; set; }
    }
}
