// <copyright file="VatRegimeVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c969939e-7511-49c6-8f43-ac1a79118046")]
    #endregion
    public partial class VatRegimeVersion : Version
    {
        #region inherited properties

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("b4b5fdc5-7533-4d7e-b91a-80a630ca2ca2")]
        [AssociationId("b76a672e-b662-47f1-86ce-729185446b61")]
        [RoleId("826e97c4-115e-473b-b99c-49643d8749d1")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Country Country { get; set; }

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
