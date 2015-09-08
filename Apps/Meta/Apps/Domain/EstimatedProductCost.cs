namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c8df7ac5-4e6f-4add-981f-f0d9a8c14e24")]
	#endregion
	[Plural("EstimatedProductCosts")]
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class EstimatedProductCostInterface: Interface
	{
		#region Allors
		[Id("2a8f919f-19f0-4b33-b8b8-26937d49d298")]
		[AssociationId("6d46215f-6af1-49b9-bc27-41de412a5b43")]
		[RoleId("0d0adab4-db9a-492b-8aaf-e40b864705aa")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Costs")]
		public RelationType Cost;

		#region Allors
		[Id("78a7ee9c-4aeb-471d-ae17-5878737f1f67")]
		[AssociationId("d4e26be2-9adc-4ded-b373-e88c7ecd7e29")]
		[RoleId("51bb9283-5e98-4a69-ae20-85460ee532d7")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("ce0f4392-cf76-49ba-a6bd-47b4e125ec61")]
		[AssociationId("acc9ae9a-8cb4-46cc-a507-db82759435d8")]
		[RoleId("5ebf8530-9a22-43d0-a1db-d976dfcbeaea")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationClass))]
		[Plural("Organisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Organisation;

		#region Allors
		[Id("d5e63839-7009-4582-8d9a-ac9591aa10c9")]
		[AssociationId("bfc2363f-b9ef-43ba-b5de-83104b9492ba")]
		[RoleId("31982d33-6240-4718-b9db-6762adb85670")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("e7942246-0343-437e-9b92-fc2d5e6438fd")]
		[AssociationId("434c6b12-146d-4f53-b1a3-5b75afaf57f2")]
		[RoleId("c763f72b-aa80-4caa-91b0-eddb949d3d34")]
		#endregion
		[Indexed]
		[Type(typeof(GeographicBoundaryInterface))]
		[Plural("GeographicBoundaries")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GeographicBoundary;



		public static EstimatedProductCostInterface Instance {get; internal set;}

		internal EstimatedProductCostInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Cost.RoleType.IsRequired = true;
            this.Currency.RoleType.IsRequired = true;
        }
    }
}