namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("ea83087e-05cc-458c-a6ba-3ce947644a0f")]
    #endregion
    public partial class ProductCategory : AccessControlledObject, UniquelyIdentifiable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("049849d5-514b-418d-8397-29db6671b4fa")]
        [AssociationId("51631226-3c9e-46a5-9748-b9ab44e36173")]
        [RoleId("11d9ba5c-1012-4442-893b-223d21ba7df7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Package Package { get; set; }

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
        [Id("25367f24-0f84-44f2-adce-ea3c082b6449")]
        [AssociationId("729444e1-4cdb-4090-a83b-bfff6d72ac95")]
        [RoleId("956368ba-ca75-4e93-be53-6c0cfc41d704")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media NoImageAvailableImage { get; set; }

        #region Allors
        [Id("2dcea42e-2c3d-483c-b514-b7bd418318ab")]
        [AssociationId("98564463-d7a9-4605-997c-2ceacb5c3302")]
        [RoleId("f8ad2d5e-eab0-4c5b-8cb7-35b3439e62e6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public ProductCategory[] Parents { get; set; }

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
        [Id("743985f3-cfee-45b5-b971-30adf46b5297")]
        [AssociationId("9bc06415-c87c-44ab-8644-6a3d53595bd1")]
        [RoleId("22e25946-7262-4fc3-a6ee-d9a25494298a")]
        #endregion
        [Size(-1)]
        [Derived]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("8af8b1b1-a711-4e98-a6a0-2948f2d1f315")]
        [AssociationId("042e65b2-6df9-4e76-91bd-7766e935cbfe")]
        [RoleId("991971a4-4ced-4cad-a7a5-48cde31f5e95")]
        #endregion
        [Size(256)]
        [Derived]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("0FB2F768-8313-450C-94AE-5F9C52B758E8")]
        [AssociationId("A9401345-9CBB-4CD7-A792-57EAEC1C5F53")]
        [RoleId("D307F6DF-1630-422F-A67C-769F9809FABC")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Required]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedNames { get; set; }

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
        [Id("b02c92d3-8b3a-4ce0-a49d-5c608a25b7d4")]
        [AssociationId("b01ed533-259c-429c-8827-c61222896b8f")]
        [RoleId("7efeb782-6278-4482-8cbb-b46d2a146e96")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public ProductCategory[] Ancestors { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}