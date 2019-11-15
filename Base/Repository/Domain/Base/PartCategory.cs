// <copyright file="PartCategory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("27A26380-7007-4A18-8054-D7A446604452")]
    #endregion
    public partial class PartCategory : UniquelyIdentifiable, Deletable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("5982BEDE-D784-4356-8523-9D1DD2B774BC")]
        [AssociationId("ED228E98-B8F2-4928-B6F7-A56CF1FCDA22")]
        [RoleId("8D97201D-03D3-4169-A5D9-DDD5953D258A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PartCategory PrimaryParent { get; set; }

        #region Allors
        [Id("67645C78-0428-4B8C-946E-1E1F9AABEA2C")]
        [AssociationId("9C3B224C-260B-41C6-A0A1-7D11448BDA00")]
        [RoleId("44032CB4-20ED-458B-A141-91C3BCE8B262")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        public PartCategory[] PrimaryAncestors { get; set; }

        #region Allors
        [Id("71F448B7-9486-4A12-8495-C61307C41924")]
        [AssociationId("D83C3C1F-7C23-4031-9412-486CB0E032BA")]
        [RoleId("81143B0E-488C-4BDC-8E7A-7E70CB19E8FD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public PartCategory[] SecondaryParents { get; set; }

        #region Allors
        [Id("F005F74D-E7E4-4488-82A0-55C0384FF255")]
        [AssociationId("9040D0DC-77FC-4F61-9406-04A9EB0C57DB")]
        [RoleId("C7AAE518-B854-45D1-B5D2-79BCEB53E3FF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public PartCategory[] Children { get; set; }

        #region Allors
        [Id("80890B4C-80D7-438F-ADC6-F9078F2A2882")]
        [AssociationId("5F18173E-76DF-4043-BCDA-C4BFCA05B582")]
        [RoleId("346DC4DD-0CD9-4128-BF92-17C04D4B26C5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public PartCategory[] Descendants { get; set; }

        #region Allors
        [Id("D129AEC4-5345-40AC-BFBF-1AF849B21544")]
        [AssociationId("1938F738-5AF2-431C-BF63-9F3D2D03BB70")]
        [RoleId("E09CE22A-5CB9-41EA-9F7D-C4850A0B6BC4")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("CF6C83F2-6CDC-4E9B-A757-79F8A71C2BD7")]
        [AssociationId("64522A77-C76F-422E-A85E-CA060CCE171D")]
        [RoleId("D1A1D0E8-3264-40CA-87B2-4CC720418966")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedNames { get; set; }

        #region Allors
        [Id("9D3D6A2A-3CA8-4A1F-B736-60268EF8F73B")]
        [AssociationId("2A7D8940-145E-4E90-BF10-15FFBC136A98")]
        [RoleId("6FB816BF-EFA2-4C7F-8940-886F97884A0D")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("7E3C4257-E2E7-4C42-A51C-26B939E9524F")]
        [AssociationId("B79B8CC2-384F-467B-AB59-6CC2599D0D89")]
        [RoleId("AA28BB55-FC01-47D9-AE98-442214D47B84")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedDescriptions { get; set; }

        #region Allors
        [Id("90BA0958-81FD-4249-A2F4-53A12DA2F33A")]
        [AssociationId("4D779492-57E9-4A7B-BBDD-2DC2D1741E83")]
        [RoleId("BC424F59-7EFC-4671-A61F-1AB07979A7D7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media CategoryImage { get; set; }

        #region Allors
        [Id("FAC3039F-B0BF-44B6-8DEF-6CDF81886148")]
        [AssociationId("C5F27EFD-0417-4DC6-B466-855D5762697F")]
        [RoleId("642055D0-0EE4-4856-A692-8A6846AADB9E")]
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
