// <copyright file="Notification.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("73dcdc68-7571-4ed1-86db-77c914fe2f62")]
    #endregion
    public partial class Notification : Deletable, Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("9a226bec-31b9-413e-bec1-8dcdf36fa6fb")]
        [AssociationId("c05db652-e6b0-485b-bcf5-9ec77a20d068")]
        [RoleId("db9f708f-ac35-49f4-a62a-9df9863da8bd")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public UniquelyIdentifiable Target { get; set; }

        #region Allors
        [Id("50b1be30-d6a9-49e8-84da-a47647e443f0")]
        [AssociationId("cb7cc442-b05b-48a5-8696-4baab7aa8cce")]
        [RoleId("3ee1975d-5019-4043-be5f-65331ef58787")]
        #endregion
        [Workspace]
        [Required]
        public bool Confirmed { get; set; }

        #region Allors
        [Id("70292962-9e0e-4b57-a710-c8ac34f65b11")]
        [AssociationId("80e4d1c5-5648-486a-a2ff-3b1690514f20")]
        [RoleId("84813900-abe0-4c24-bd2e-5b0d6be5ab6c")]
        [Size(1024)]
        #endregion
        [Workspace]
        [Required]
        public string Title { get; set; }

        #region Allors
        [Id("e83600fc-5411-4c72-9903-80a3741a9165")]
        [AssociationId("1da1555c-fee6-4084-be37-57a6f58fe591")]
        [RoleId("a6f6ed43-b0ab-462f-9be9-fad58d2e8398")]
        [Size(-1)]
        #endregion
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("458a8223-9c0f-4475-93c0-82d5cc133f1b")]
        [AssociationId("8d657749-a823-422b-9e29-041453ccb267")]
        [RoleId("d100a845-df65-4f06-96b8-dcaeb9c3e205")]
        [Derived]
        [Indexed]
        #endregion
        [Workspace]
        [Required]
        public DateTime DateCreated { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion

        #region Allors
        [Id("B445FC66-27AF-4D45-ADA8-4F1409EBBE72")]
        #endregion
        [Workspace]
        public void Confirm() { }
    }
}
