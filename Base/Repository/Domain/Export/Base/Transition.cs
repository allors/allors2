// <copyright file="Transition.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("a7e490c0-ce29-4298-97c4-519904bb755a")]
    #endregion
    public partial class Transition : Object
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("c6ee1a42-05fa-462b-b04f-811f01c6b646")]
        [AssociationId("ae7fa215-20bb-4472-9d25-ee3174f40fdb")]
        [RoleId("e79fa7b8-870a-4a6e-8522-bb39437e0650")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public ObjectState[] FromStates { get; set; }
        #region Allors
        [Id("dd19e7f8-83b7-4ff1-b475-02c4296b47e4")]
        [AssociationId("c88c9ab2-af38-45ca-9caa-fcb5715da129")]
        [RoleId("c68eb959-1b2c-48a7-b15a-944a576944ef")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public ObjectState ToState { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
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
