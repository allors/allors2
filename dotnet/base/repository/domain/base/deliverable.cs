// <copyright file="Deliverable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("68a6803d-0e65-4141-ac51-25f4c2e49914")]
    #endregion
    public partial class Deliverable : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("d7322009-e68f-4635-bc0e-1c0b5a46de62")]
        [AssociationId("953cd640-51dd-4543-a751-242c7e39b596")]
        [RoleId("38bd223e-54ee-455d-8da5-3106029e1fbe")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }

        #region Allors
        [Id("dfd5fb95-50ee-48a5-942b-75752f78a615")]
        [AssociationId("fea5e2c3-b8fa-488d-aba6-641176652430")]
        [RoleId("50499eba-a2b0-4ad2-8dc6-72eb2d1997a7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public DeliverableType DeliverableType { get; set; }

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
