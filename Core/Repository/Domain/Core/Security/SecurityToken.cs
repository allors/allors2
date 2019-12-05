// <copyright file="SecurityToken.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("a53f1aed-0e3f-4c3c-9600-dc579cccf893")]
    #endregion
    public partial class SecurityToken : UniquelyIdentifiable, Deletable
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("6503574b-8bab-4da8-a19d-23a9bcffe01e")]
        [AssociationId("cae9e5c2-afa1-46f4-b930-69d4e810038f")]
        [RoleId("ab2b4b9c-87dd-4712-b123-f5f9271c6193")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public AccessControl[] AccessControls { get; set; }

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

        public void Delete() { }
        #endregion
    }
}
