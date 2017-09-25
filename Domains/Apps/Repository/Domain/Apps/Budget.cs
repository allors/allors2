namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("ebd4da8c-b86a-4317-86b9-e90a02994dcc")]
    #endregion
    public partial interface Budget : Period, Commentable, UniquelyIdentifiable, Transitional
    {
        #region Allors
        [Id("1848add9-ab90-4191-b7f1-eb392be3ec4e")]
        [AssociationId("8232c215-e592-4ec7-8c44-391c917b7e89")]
        [RoleId("5e27d83d-a601-4101-b4dd-7eef98de82e8")]
        #endregion
        [Required]
        [Size(256)]
        string Description { get; set; }
        
        #region Allors
        [Id("1c3dd3b4-b514-4a42-965f-d3200325d78c")]
        [AssociationId("dccc1ed1-0cac-4e25-a7ee-5848af5b390e")]
        [RoleId("684c491e-c764-4d83-a11f-d3cf80d671ad")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        BudgetRevision[] BudgetRevisions { get; set; }
        
        #region Allors
        [Id("2163a044-c967-4137-b1d0-dfd3fac80869")]
        [AssociationId("3ec284eb-944c-4ff0-8e24-9be0ceeda22a")]
        [RoleId("f02d72a7-2547-44dc-bb8b-42e58afe186d")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        BudgetStatus[] BudgetStatuses { get; set; }
        
        #region Allors
        [Id("494d04ef-aafc-4482-a5c2-4ec9fa93d158")]
        [AssociationId("eda25f81-bba9-4e23-9074-4e22338ace23")]
        [RoleId("d2a2990a-2966-4302-8c18-0884915f9d33")]
        #endregion
        [Size(256)]
        string BudgetNumber { get; set; }
        
        #region Allors
        [Id("59cbc253-e17d-4405-bea8-09ad420bf8bc")]
        [AssociationId("6f6d9d35-daf5-4a79-85ce-d662cd7ec2d4")]
        [RoleId("a6ec675f-c28a-470e-9923-e623e0ca9c58")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        BudgetObjectState CurrentObjectState { get; set; }


        #region Allors
        [Id("834432b1-65b2-4499-a83d-71f0db6e177b")]
        [AssociationId("b7f09631-6b4c-417d-ba12-115d07d9d6f5")]
        [RoleId("b9ba1402-ce06-4bdd-9290-165ff8e555d2")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        BudgetReview[] BudgetReviews { get; set; }


        #region Allors
        [Id("d4d205b5-6f23-41f5-93fc-08d4d9ad0727")]
        [AssociationId("181fd812-5a57-44d5-92c7-70755df1c9e3")]
        [RoleId("516885f8-6aee-4e63-bd38-3134ed753e28")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        BudgetStatus CurrentBudgetStatus { get; set; }


        #region Allors
        [Id("f6078f5b-036f-45de-ab4f-fb26b6939d11")]
        [AssociationId("ba8edec9-a429-482d-bfbd-4f7fd419eaf7")]
        [RoleId("9b9e4779-bb7d-4edb-b432-eab76472135a")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        BudgetItem[] BudgetItems { get; set; }
        
        #region Allors
        [Id("A6ED3503-571A-4800-B1BE-379CE197584F")]
        #endregion
        void Close();

        #region Allors
        [Id("B33D83BD-D1E5-4544-9B18-999EF78E4AE2")]
        #endregion
        void Reopen();
    }
}