// <copyright file="Bank.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors

    [Id("a24a8e12-7067-4bfb-8fc0-225a824d1a05")]

    #endregion

    public partial class Bank : Object
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("28723704-3a61-445a-b14e-c757ebbf8d66")]
        [AssociationId("86b555ec-72f1-4ed6-b161-56d23508cf99")]
        [RoleId("17dc868b-9a11-41c1-9366-b10f47d1fe3f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Media Logo { get; set; }

        #region Allors
        [Id("354e114f-5d6b-4883-8e58-5c7a39878b6d")]
        [AssociationId("5c30c485-8f98-4a6d-8e05-3774331d9e7a")]
        [RoleId("ed87ce26-a306-4590-8901-7b4fca4e2f57")]
        #endregion
        [Required]
        [Size(256)]
        public string Bic { get; set; }

        #region Allors
        [Id("a7851af8-38cd-4785-b81c-fb2fa403d9f6")]
        [AssociationId("f7460d4e-1094-46af-b04a-46115c2fee6a")]
        [RoleId("92429a4b-9166-4e40-a356-caedaf296e23")]
        #endregion
        [Size(256)]
        public string SwiftCode { get; set; }

        #region Allors
        [Id("d3a11d21-0232-48a0-b784-c111ad05f5da")]
        [AssociationId("0c8f4f92-50c5-4440-ae4e-e1734d7fdc60")]
        [RoleId("627538dd-fac1-44a0-83c0-220b65440365")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Country Country { get; set; }

        #region Allors
        [Id("d4191223-d9be-4cbb-b2ad-ee0844dcae87")]
        [AssociationId("dc80650c-b20f-468f-8d3f-5410a7632961")]
        [RoleId("85b6a787-7c26-42a6-aef9-d8b685ff97f6")]
        #endregion
        [Required]
        [Size(256)]
        public string Name { get; set; }

        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion
    }
}
