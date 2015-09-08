namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c5578565-c07a-4dc1-8381-41955db364e2")]
	#endregion
	[Plural("OrderAdjustments")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class OrderAdjustmentInterface: Interface
	{
		#region Allors
		[Id("4e7cbdda-9f19-44dd-bbef-6cab5d92a8a3")]
		[AssociationId("5ccd492c-cf29-468b-b99d-126a9573e573")]
		[RoleId("7388d1a3-f24a-4c41-b57c-938160b3d1a6")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;

		#region Allors
		[Id("78d6de86-0f4d-4d8e-a9a6-4730668fa754")]
		[AssociationId("51d96df2-1e92-4ea2-8ec7-e918d5781ae7")]
		[RoleId("933a70e0-0fa0-42cd-a4d5-b3eb10b57802")]
		#endregion
		[Indexed]
		[Type(typeof(VatRateClass))]
		[Plural("VatRates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType VatRate;

		#region Allors
		[Id("bc1ad594-88b6-4176-994c-a52be672f06d")]
		[AssociationId("ebc960bf-dd8c-4854-afec-185b260315e9")]
		[RoleId("9d2f66e2-0bbd-46ab-b65b-43e6b38383b9")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Percentages")]
		public RelationType Percentage;



		public static OrderAdjustmentInterface Instance {get; internal set;}

		internal OrderAdjustmentInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}