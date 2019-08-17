// <copyright file="CreditCardCompany.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("86d934de-a5cf-46d3-aad3-2626c43ebc85")]
    #endregion
    public partial class CreditCardCompany : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("05860987-77be-4d8d-823d-99dd0e2cc822")]
        [AssociationId("002eff4d-2bcc-40bb-b311-7ae86207bdc7")]
        [RoleId("c9fe6f93-933e-4859-aaa2-ef3f5e2c8b44")]
        #endregion
        [Required]
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
