// <copyright file="PartCategory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("81B8CCCB-E8F7-416C-8372-877B82956F10")]
    #endregion
    public partial class PartCategory : UniquelyIdentifiable, Deletable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("674BF176-971B-40FB-AAD9-9D5606950B73")]
        [AssociationId("75D480CF-D6EC-49E9-8D46-F91A8FFBBDDD")]
        [RoleId("894DF94F-7112-402E-878F-E19194BC67C3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PartCategory PrimaryParent { get; set; }

        #region Allors
        [Id("DFE04587-B255-434F-A0BC-25FC9BA47AD6")]
        [AssociationId("7B8AD268-2B7D-4852-865F-5D498C66736B")]
        [RoleId("B8986EE4-9FB0-4745-8021-20B4EABF7883")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        public PartCategory[] PrimaryAncestors { get; set; }

        #region Allors
        [Id("D06C2840-DC85-435E-8852-E66EA8C3C7DA")]
        [AssociationId("B21E6B01-CF05-43B4-B683-BF25FDB1AC22")]
        [RoleId("2F0BF64A-5FE2-4FE4-89E7-E7A08BD161D6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public PartCategory[] SecondaryParents { get; set; }

        #region Allors
        [Id("EFEF3C19-F36D-441B-A134-18C287A56CBD")]
        [AssociationId("35D519F4-213A-45E2-9F9A-52CF5D984E0D")]
        [RoleId("F1ABC5EF-F3FA-4BC6-80B4-4C82DB5FBB53")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public PartCategory[] Children { get; set; }

        #region Allors
        [Id("A5603B32-74AA-4F33-9413-91D9D3EEEC34")]
        [AssociationId("280C03F4-A484-47CF-8FDA-EEBFFA87F66B")]
        [RoleId("9E99C825-4B1E-448F-8E06-B9E7CCB31225")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public PartCategory[] Descendants { get; set; }

        #region Allors
        [Id("4C66BBBF-7B21-4EE4-A7FE-C2384CCAA71D")]
        [AssociationId("90BDF07C-0F60-4655-8F09-6B59347DC493")]
        [RoleId("069A762E-D9A8-47BB-BB4F-67128B7D680B")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("1C0C4A04-FB3E-4829-8881-05AE393469A3")]
        [AssociationId("0DB1AEA3-12F4-4556-99C3-B5E5C9E623C2")]
        [RoleId("F378D7FF-F898-46AD-B8A2-0074077A082D")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedNames { get; set; }

        #region Allors
        [Id("2928B33B-7E55-44BC-9083-E6B53902C03B")]
        [AssociationId("2DA26EB7-43E0-4C7C-9BCC-8433DB3CA046")]
        [RoleId("0042D1C5-32AD-46AC-AC31-E05CE68313AC")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("766D1DEA-4377-4AB9-A990-D4D49DD2A2D6")]
        [AssociationId("A20397B2-776F-4271-ABF1-36728161536C")]
        [RoleId("E01D041D-662F-4A90-A3A2-0ED9C7205DAB")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedDescriptions { get; set; }

        #region Allors
        [Id("B49ED81B-AFE8-4AAE-A8B1-2BB12FEF4151")]
        [AssociationId("4C153688-DAAA-4C67-AF58-BD01946330D5")]
        [RoleId("6AAC81DF-A4AD-4557-B68B-AE6177F7C23A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media CategoryImage { get; set; }

        #region Allors
        [Id("DE88E6AB-B8B3-4E84-BA42-630DE3D4C17D")]
        [AssociationId("15FD1707-C1B7-4706-8FDE-FB68B3016B0B")]
        [RoleId("7520D343-39CF-41CD-B7C6-F23FA86DE83D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public Part[] Parts { get; set; }

        #region inherited methods

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
