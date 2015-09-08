namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2a79b7f0-5998-4d52-b995-8412df939098")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("SalesRepRevenueHistories")]
	public partial class SalesRepRevenueHistoryClass : Class
	{
		#region Allors
		[Id("0d2277e4-470d-487a-9276-3972d57c8512")]
		[AssociationId("a86b2a21-c5d1-4ee5-9039-e5b3fcfd3d2d")]
		[RoleId("8ff9fb8e-a85b-4a47-b591-c882614c767f")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("5dc09d22-d541-4088-a715-9fc65d24453f")]
		[AssociationId("2a2dccee-642d-4336-9447-8304da2f8c79")]
		[RoleId("b4774899-e3cc-4990-9974-7638b9e60d9b")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("SalesReps")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalesRep;

		#region Allors
		[Id("a38139e6-a227-478b-8924-7d62b73c06d8")]
		[AssociationId("78157141-6c25-44ce-bd6d-f64f07ae2baa")]
		[RoleId("3ec55f87-0dde-48d0-878c-834add5f2a9c")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;

		#region Allors
		[Id("b3e41872-5488-4ca6-b3c1-3b1ff058b6bb")]
		[AssociationId("96ab29d6-1aca-48a3-b4d9-4a1123d50f01")]
		[RoleId("13e42a5b-76e7-4c0a-a3bd-0fe8e42c52a0")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Revenues")]
		public RelationType Revenue;



		public static SalesRepRevenueHistoryClass Instance {get; internal set;}

		internal SalesRepRevenueHistoryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}