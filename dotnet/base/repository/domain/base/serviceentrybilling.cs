// <copyright file="ServiceEntryBilling.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("2be4075a-c7e3-4a38-a045-7910f85b3e46")]
    #endregion
    public partial class ServiceEntryBilling : Object, Deletable
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("2fb9d650-0a28-4a39-8427-8c12bc4a20a1")]
        [AssociationId("c91e3796-6b23-4aa7-992b-ac15da334eae")]
        [RoleId("7ca6affa-7d4f-45bd-88e2-a0fa1cad4ad7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public ServiceEntry ServiceEntry { get; set; }

        #region Allors
        [Id("a8c707fb-98c1-43b1-99a3-9464cb25ea5f")]
        [AssociationId("284bf54c-8305-4892-ad00-f4975e155522")]
        [RoleId("570acec5-b62e-4abc-bb1d-000fb70bc2fe")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InvoiceItem InvoiceItem { get; set; }

        #region inherited methods

        public Restriction[] Restrictions { get; set; }

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
