namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0a7ac589-946b-4d49-b7e0-7e0b9bc90111")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Brands")]
	public partial class BrandClass : Class
	{
		#region Allors
		[Id("08b0dfc6-2e2f-4e40-96d1-851e26b38e8d")]
		[AssociationId("8b22eb1a-ecc3-4e0d-898c-4a5c651f1d2c")]
		[RoleId("2f85b937-569a-4317-ac4a-aa1e89541a20")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("25729ffa-4f97-464a-9b34-fe1661e0d932")]
		[AssociationId("ae59e80c-289d-487f-996c-c83615d8750d")]
		[RoleId("f02d54d1-8ed9-4f63-96b5-397cb4b761d2")]
		#endregion
		[Indexed]
		[Type(typeof(ProductCategoryClass))]
		[Plural("ProductCategories")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType ProductCategory;



		public static BrandClass Instance {get; internal set;}

		internal BrandClass() : base(MetaPopulation.Instance)
        {
        }
        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}