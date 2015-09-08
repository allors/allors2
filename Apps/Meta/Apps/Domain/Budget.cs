namespace Allors.Meta
{
	#region Allors
	[Id("ebd4da8c-b86a-4317-86b9-e90a02994dcc")]
	#endregion
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
  	public partial class BudgetInterface: Interface
	{
        #region Allors
        [Id("3E913270-98BC-4A29-8C54-AD94B78D62A3")]
        #endregion
        public MethodType Close;

        #region Allors
        [Id("4D8FD306-049E-4909-AFA8-91A615B76314")]
        #endregion
        public MethodType Reopen;

        #region Allors
        [Id("1848add9-ab90-4191-b7f1-eb392be3ec4e")]
		[AssociationId("8232c215-e592-4ec7-8c44-391c917b7e89")]
		[RoleId("5e27d83d-a601-4101-b4dd-7eef98de82e8")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("1c3dd3b4-b514-4a42-965f-d3200325d78c")]
		[AssociationId("dccc1ed1-0cac-4e25-a7ee-5848af5b390e")]
		[RoleId("684c491e-c764-4d83-a11f-d3cf80d671ad")]
		#endregion
		[Indexed]
		[Type(typeof(BudgetRevisionClass))]
		[Plural("BudgetRevisions")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType BudgetRevision;

		#region Allors
		[Id("2163a044-c967-4137-b1d0-dfd3fac80869")]
		[AssociationId("3ec284eb-944c-4ff0-8e24-9be0ceeda22a")]
		[RoleId("f02d72a7-2547-44dc-bb8b-42e58afe186d")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(BudgetStatusClass))]
		[Plural("BudgetStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType BudgetStatus;

		#region Allors
		[Id("494d04ef-aafc-4482-a5c2-4ec9fa93d158")]
		[AssociationId("eda25f81-bba9-4e23-9074-4e22338ace23")]
		[RoleId("d2a2990a-2966-4302-8c18-0884915f9d33")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("BudgetNumbers")]
		public RelationType BudgetNumber;

		#region Allors
		[Id("59cbc253-e17d-4405-bea8-09ad420bf8bc")]
		[AssociationId("6f6d9d35-daf5-4a79-85ce-d662cd7ec2d4")]
		[RoleId("a6ec675f-c28a-470e-9923-e623e0ca9c58")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(BudgetObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("834432b1-65b2-4499-a83d-71f0db6e177b")]
		[AssociationId("b7f09631-6b4c-417d-ba12-115d07d9d6f5")]
		[RoleId("b9ba1402-ce06-4bdd-9290-165ff8e555d2")]
		#endregion
		[Indexed]
		[Type(typeof(BudgetReviewClass))]
		[Plural("BudgetReviews")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType BudgetReview;

		#region Allors
		[Id("d4d205b5-6f23-41f5-93fc-08d4d9ad0727")]
		[AssociationId("181fd812-5a57-44d5-92c7-70755df1c9e3")]
		[RoleId("516885f8-6aee-4e63-bd38-3134ed753e28")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(BudgetStatusClass))]
		[Plural("CurrentBudgetStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentBudgetStatus;

		#region Allors
		[Id("f6078f5b-036f-45de-ab4f-fb26b6939d11")]
		[AssociationId("ba8edec9-a429-482d-bfbd-4f7fd419eaf7")]
		[RoleId("9b9e4779-bb7d-4edb-b432-eab76472135a")]
		#endregion
		[Indexed]
		[Type(typeof(BudgetItemClass))]
		[Plural("BudgetItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType BudgetItem;

		public static BudgetInterface Instance {get; internal set;}

		internal BudgetInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}