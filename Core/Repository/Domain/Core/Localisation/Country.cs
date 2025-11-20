// <copyright file="Country.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("c22bf60e-6428-4d10-8194-94f7be396f28")]
    #endregion
    [Plural("Countries")]
    public partial class Country : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("62009cef-7424-4ec0-8953-e92b3cd6639d")]
        [AssociationId("323173ee-385c-4f74-8b78-ff05735460f8")]
        [RoleId("4ca5a640-5d9e-4910-95ed-6872c7ea13d2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Currency Currency { get; set; }

        #region Allors
        [Id("f93acc4e-f89e-4610-ada9-e58f21c165bc")]
        [AssociationId("ea0efe67-89f2-4317-97e7-f0e14358e718")]
        [RoleId("4fe997d6-d221-432b-9f09-4f77735c109b")]
        #endregion
        [Required]
        [Unique]
        [Size(2)]
        [Workspace]
        public string IsoCode { get; set; }

        #region Allors
        [Id("6b9c977f-b394-440e-9781-5d56733b60da")]
        [AssociationId("6e3532ae-3528-4114-9274-54fc08effd0d")]
        [RoleId("60f1f9a3-13d1-485f-bc77-fda1f9ef1815")]
        #endregion
        [Indexed]
        [Unique]
        [Size(256)]
        [Required]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("8236a702-a76d-4bb5-9afd-acacb1508261")]
        [AssociationId("9b682612-50f9-43f3-abde-4d0cb5156f0d")]
        [RoleId("99c52c13-ef50-4f68-a32f-fef660aa3044")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedNames { get; set; }

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
