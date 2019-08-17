// <copyright file="OrderValue.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a3ca36e6-960d-4e3a-96d0-6ca1d71d05d7")]
    #endregion
    public partial class OrderValue : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("077a33bc-a822-4a23-918c-7fcaacdc61d1")]
        [AssociationId("f38c5851-7187-4f53-8eaf-85edee7e733d")]
        [RoleId("7ee1e68b-5bb5-4e72-b63d-83132346a503")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal ThroughAmount { get; set; }
        #region Allors
        [Id("b25816e8-4b0c-4857-907f-7a391df2c55e")]
        [AssociationId("017aab24-a93c-4654-bc89-96e075d13c08")]
        [RoleId("eedd52f8-0713-428e-b10a-a7da99b967aa")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal FromAmount { get; set; }

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
