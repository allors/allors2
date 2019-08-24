// <copyright file="NewsItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("d50ffc20-9e2d-4362-8e3f-b54d7368d487")]
    #endregion
    public partial class NewsItem : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("1a86dc14-eadc-4aad-83c2-238e31a20658")]
        [AssociationId("f8d3058b-3e81-4da8-a29f-52dd267e1733")]
        [RoleId("ae0eba55-6aaf-4ed4-a784-006f5bf95f49")]
        #endregion

        public bool IsPublished { get; set; }

        #region Allors
        [Id("2f1736ea-0e74-43a9-b047-cc37bc9618fa")]
        [AssociationId("c1202b23-5507-43ea-849d-94cd46392927")]
        [RoleId("0add2328-35a3-4286-bd0e-258a47756ce5")]
        #endregion
        [Size(256)]

        public string Title { get; set; }

        #region Allors
        [Id("369f8b36-1bb8-45b6-b02d-6cd7c126cb54")]
        [AssociationId("bc2dd1eb-0f39-4717-bd3e-bfcaee6e0a0c")]
        [RoleId("c43bf977-c5de-44f9-99a9-bb8e9dd96122")]
        #endregion

        public int DisplayOrder { get; set; }

        #region Allors
        [Id("372331ef-70a4-4a67-8f85-a0907ace9194")]
        [AssociationId("39e20d75-09f5-4692-a16a-86c2b284e0fa")]
        [RoleId("0ad8a7a1-3466-452c-ac7f-5df53627ae5f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Locale Locale { get; set; }

        #region Allors
        [Id("4a03f057-339f-4dd4-ac89-3b97d27d2170")]
        [AssociationId("ca816c14-6aaf-4ed8-b140-f9c941e4f769")]
        [RoleId("687c0be7-138d-4ea3-a87f-58df8ac7e60d")]
        #endregion
        [Size(-1)]

        public string LongText { get; set; }

        #region Allors
        [Id("7aee11d0-f9b4-450d-83b8-357811e99246")]
        [AssociationId("2ddcf225-907a-4d99-921c-f61893aa7ac8")]
        [RoleId("a08e2201-208f-4823-a814-a498aa0db9a5")]
        #endregion
        [Size(-1)]

        public string Text { get; set; }

        #region Allors
        [Id("a184408c-a1b0-47b2-821a-a2ab643b523e")]
        [AssociationId("213a7484-741a-4a2b-b765-3bc1b8427a64")]
        [RoleId("55094ebe-bf49-44f4-bb0f-5722eca4ae90")]
        #endregion

        public DateTime Date { get; set; }

        #region Allors
        [Id("d29e707f-66dc-4fbf-aba4-63473498dd4b")]
        [AssociationId("b977e1eb-5a75-4f36-8381-6b49615e7969")]
        [RoleId("5fe8e6f1-a992-452b-8711-0a9ed007ef2e")]
        #endregion

        public bool Announcement { get; set; }

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
