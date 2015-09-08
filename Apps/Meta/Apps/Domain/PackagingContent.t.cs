namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1c05a785-2de1-48fa-813f-6e740f6f7cec")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PackagingContents")]
	public partial class PackagingContentClass : Class
	{
		#region Allors
		[Id("316a8ff4-1073-486e-ad62-5bee3d3504d2")]
		[AssociationId("c2970739-17c4-488a-8f12-9e35ad72d311")]
		[RoleId("536e372d-5062-418a-b17e-752ebf32d430")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentItemClass))]
		[Plural("ShipmentItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipmentItem;

		#region Allors
		[Id("ca8bcf75-c40e-4d73-8d0c-f35d1005f73b")]
		[AssociationId("a97a1fd4-6d74-424c-aab4-909bdd198856")]
		[RoleId("db47bbd5-e9d8-4dea-801f-bae1c49fe67c")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Quantities")]
		public RelationType Quantity;



		public static PackagingContentClass Instance {get; internal set;}

		internal PackagingContentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}