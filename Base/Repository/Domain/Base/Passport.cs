// <copyright file="Passport.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("827bc38b-6570-41d7-8ae1-f1bbdf4409f9")]
    #endregion
    public partial class Passport : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("85036007-8e01-4d90-9cfe-7b9c25e43537")]
        [AssociationId("b1235d10-b895-40dc-bc99-680d08b4cef2")]
        [RoleId("9a1e96ae-0a56-4812-ac52-1c142afd61c2")]
        #endregion

        public DateTime IssueDate { get; set; }

        #region Allors
        [Id("dd30acd3-2e7b-49e6-9fcd-04cfdafb62d0")]
        [AssociationId("cb5a7d75-b938-4451-9896-b661b1828fab")]
        [RoleId("bc010471-bb69-4735-8a86-25d1a8528d34")]
        #endregion

        public DateTime ExpiriationDate { get; set; }

        #region Allors
        [Id("eb3cdf1a-d577-46ff-9d0e-d709c6e7d9d9")]
        [AssociationId("8d5d9376-24e0-486f-84fb-f242bfaee585")]
        [RoleId("b5ea6bb3-6498-46c6-b579-3839f5effbe1")]
        #endregion
        [Required]
        [Unique]
        [Size(256)]

        public string Number { get; set; }

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
