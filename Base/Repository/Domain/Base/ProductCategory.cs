// <copyright file="ProductCategory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("ea83087e-05cc-458c-a6ba-3ce947644a0f")]
    #endregion
    public partial class ProductCategory : UniquelyIdentifiable, Deletable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("1F5775EF-9440-405B-8A7D-2A6460D8BCAF")]
        [AssociationId("E096110D-CB3D-4EC3-A79F-51BC7A9312C9")]
        [RoleId("85CC1836-08A2-437F-B650-10886FB86A40")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation InternalOrganisation { get; set; }

        #region Allors
        [Id("22b7b6ef-7adf-424d-a675-d5338478ed44")]
        [AssociationId("b80ca91e-846f-4af6-a3a7-b361ef7b6058")]
        [RoleId("55f938db-31e5-468c-90ad-1f7db319afce")]
        #endregion
        [Indexed]
        [Size(256)]
        [Workspace]
        public string Code { get; set; }

        #region Allors
        [Id("511A0C9B-46C0-4ED8-8C6E-280FF4634076")]
        [AssociationId("B4793F39-2BA8-4848-B955-697FE34B80DB")]
        [RoleId("830F63F0-DB64-4209-A649-5118C4F36233")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ProductCategory PrimaryParent { get; set; }

        #region Allors
        [Id("29B96BB3-D121-405C-AFD5-90171729002E")]
        [AssociationId("AC465B3B-4C93-4357-9371-17AAA79F4322")]
        [RoleId("57EA4254-8922-465C-92A2-6FB325DE682F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        public ProductCategory[] PrimaryAncestors { get; set; }

        #region Allors
        [Id("062A7D3C-74C9-46C8-9332-C239C13B4200")]
        [AssociationId("7AE53081-3AF9-4801-810F-6103B1184E10")]
        [RoleId("9F202905-CE44-44DF-88C5-524E955DF1FD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public ProductCategory[] PreviousSecondaryParents { get; set; }

        #region Allors
        [Id("16F70B37-079A-47AE-B883-8F1E1A9E345F")]
        [AssociationId("B37765CA-61AC-4AC6-8524-EBC69B1E3333")]
        [RoleId("F743AED0-4939-4722-AF67-34166A20F05A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public ProductCategory[] SecondaryParents { get; set; }

        #region Allors
        [Id("6ad49c7d-8c4e-455b-8073-a5ef72e92725")]
        [AssociationId("a1d92298-5c2e-42eb-bf1b-1e15a07f1eac")]
        [RoleId("6b6cf3e5-c1ca-4502-ad27-85c33db1f183")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public ProductCategory[] Children { get; set; }

        #region Allors
        [Id("6AB3E5EC-FC02-4F2D-B5EB-4EFC50E2B33B")]
        [AssociationId("DF6664EF-1A6A-4B28-AB01-8B7E16B8E3B8")]
        [RoleId("8D87B6C3-D86C-464F-9B67-7BDC24F39F75")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public ProductCategory[] Descendants { get; set; }

        #region Allors
        [Id("8af8b1b1-a711-4e98-a6a0-2948f2d1f315")]
        [AssociationId("042e65b2-6df9-4e76-91bd-7766e935cbfe")]
        [RoleId("991971a4-4ced-4cad-a7a5-48cde31f5e95")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("97CB34DD-4E6A-4DCF-90B4-50071752B2D8")]
        [AssociationId("BDE30913-CEFC-4044-87D9-E032B03BF08A")]
        [RoleId("022449C3-033B-4629-B343-363091E11A5B")]
        #endregion
        [Workspace]
        public string DisplayName { get; set; }

        #region Allors
        [Id("0FB2F768-8313-450C-94AE-5F9C52B758E8")]
        [AssociationId("A9401345-9CBB-4CD7-A792-57EAEC1C5F53")]
        [RoleId("D307F6DF-1630-422F-A67C-769F9809FABC")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedNames { get; set; }

        #region Allors
        [Id("743985f3-cfee-45b5-b971-30adf46b5297")]
        [AssociationId("9bc06415-c87c-44ab-8644-6a3d53595bd1")]
        [RoleId("22e25946-7262-4fc3-a6ee-d9a25494298a")]
        #endregion
        [Size(-1)]
        [Workspace]
        [MediaType("text/markdown")]
        public string Description { get; set; }

        #region Allors
        [Id("40C3BD4D-C947-49F6-A5FA-A01398DB9E8A")]
        [AssociationId("677668F9-5D33-4BEE-B5BA-E183C38FEE6B")]
        [RoleId("F16B5C11-594E-43B0-A442-39E98C567391")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedDescriptions { get; set; }

        #region Allors
        [Id("9f50cbbc-d0af-46e6-8e04-2bfb0bf1facf")]
        [AssociationId("4fe64d4c-747c-4e8f-a657-8174eb8e0b73")]
        [RoleId("bdd11ee4-ade5-46f3-a2b1-2fbb0261ae14")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media CategoryImage { get; set; }

        #region Allors
        [Id("7B219D9E-0234-4F34-884D-D092573F6172")]
        [AssociationId("54730CB3-015E-4363-8937-37966B8293BD")]
        [RoleId("C15FF1FD-5FDD-42F4-8CE6-3BF240E1F4DC")]
        [Indexed]
        #endregion
        [Workspace]
        [Required]
        [Multiplicity(Multiplicity.ManyToOne)]
        public Scope CatScope { get; set; }

        #region Allors
        [Id("2C9927CA-BA9C-4F4A-8BE5-19523D9FDFA2")]
        [AssociationId("770C78CF-7766-4B9C-83C7-CDE38E128836")]
        [RoleId("6DBF75F3-A40B-4020-96CD-26CF46D45C93")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public Product[] Products { get; set; }

        #region Allors
        [Id("293A6FED-2EFD-464F-9FCB-5C24E74DCE80")]
        [AssociationId("00A1038B-DA0E-4FA4-8458-46D3B6420AA9")]
        [RoleId("E3EE43F8-81B5-45EC-BF82-987EC4D45344")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public Product[] AllProducts { get; set; }

        #region Allors
        [Id("43875418-7375-41BD-B6B3-1D091F98AF98")]
        [AssociationId("7994D9CC-87E2-4D58-A2ED-083F6FF46C02")]
        [RoleId("ACD2F850-D3CE-4F22-B3E1-00838E825D5D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public Part[] AllParts { get; set; }

        #region Allors
        [Id("5889B718-100F-4444-AA5D-3B56FD33AD91")]
        [AssociationId("BE94D710-A504-4C7E-9325-9AA36C0D208B")]
        [RoleId("690C5D0B-25A3-4CA0-A5F1-D583C928456F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public SerialisedItem[] AllSerialisedItemsForSale { get; set; }

        #region Allors
        [Id("A34B5082-8B4C-41D7-B1C2-DC42D8805BE7")]
        [AssociationId("9FDAC4C4-D2E9-4B9E-9B03-00E458059F5C")]
        [RoleId("DAFF3F86-0BDF-4F77-AA69-FEBA6A3B2235")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public NonSerialisedInventoryItem[] AllNonSerialisedInventoryItemsForSale { get; set; }

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
