// <copyright file="PartRevision.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("22f87630-11dd-480e-a721-9836af7685b1")]
    #endregion
    public partial class PartRevision : Period, Object
    {
        #region inherited properties
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("6d1b4cec-abff-46db-a446-0f8889426b28")]
        [AssociationId("82cf09e8-535f-45fe-876e-484dfb3ea102")]
        [RoleId("946a84d0-36f8-4805-bbdd-a0779c9d008c")]
        #endregion
        [Size(-1)]

        public string Reason { get; set; }
        #region Allors
        [Id("84561abd-08bc-4d28-b25c-22787d8bd7f0")]
        [AssociationId("4f700281-794b-4250-8bbe-f4fbbbcf8243")]
        [RoleId("8c408bc0-82f2-4343-93d2-87047c024ef9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Part SupersededByPart { get; set; }
        #region Allors
        [Id("8a064340-def3-4d9f-89d6-3325b8a41f4d")]
        [AssociationId("6c674199-8f5f-469c-8f94-f35d64304968")]
        [RoleId("190e180b-cf6f-485d-80b2-9042e0fe04a7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Part Part { get; set; }

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
