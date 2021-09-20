// <copyright file="UserGroup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("60065f5d-a3c2-4418-880d-1026ab607319")]
    #endregion
    public partial class UserGroup : UniquelyIdentifiable, Object
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("585bb5cf-9ba4-4865-9027-3667185abc4f")]
        [AssociationId("1e2d1e31-ed80-4435-8850-7663d9c5f41d")]
        [RoleId("c552f0b7-95ce-4d45-aaea-56bc8365eee4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public User[] Members { get; set; }

        #region Allors
        [Id("e94e7f05-78bd-4291-923f-38f82d00e3f4")]
        [AssociationId("75859e2c-c1a3-4f4c-bb37-4064d0aa81d0")]
        [RoleId("9d3c1eec-bf10-4a79-a37f-bc6a20ff2a79")]
        #endregion
        [Indexed]
        [Required]
        [Unique]
        [Size(256)]
        public string Name { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
