namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d204e616-039c-40c8-81cc-18f3a7345d99")]
	#endregion
	[Plural("PartBillOfMaterials")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(PeriodInterface))]

  	public partial class PartBillOfMaterialInterface: Interface
	{
		#region Allors
		[Id("06c3a64a-ef2c-44a0-81ee-1335842cf844")]
		[AssociationId("738ee8fd-307a-4d12-a0fc-238640386eee")]
		[RoleId("e0145603-3f58-46f5-8348-77ad4d211543")]
		#endregion
		[Indexed]
		[Type(typeof(PartInterface))]
		[Plural("Parts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Part;

		#region Allors
		[Id("24de2b73-c51b-47b5-bd80-2022c0e37841")]
		[AssociationId("a6dc16b1-6c02-4060-9f64-982d09ffe5dc")]
		[RoleId("f3a70021-f5af-4493-b71c-74c65649a6c1")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("Instructions")]
		public RelationType Instruction;

		#region Allors
		[Id("ac18525c-57ef-4a11-a775-e27c397b334c")]
		[AssociationId("2a043325-46cb-4219-a580-e71efe6814b5")]
		[RoleId("f526f282-0f15-4e98-af8f-9e8d658c4d38")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("QuantitiesUsed")]
		public RelationType QuantityUsed;

		#region Allors
		[Id("eb1b2313-df9b-407d-9cf9-617d58c6f4be")]
		[AssociationId("9d9c4b58-8144-4d64-92ca-b81abecc5f40")]
		[RoleId("8f188307-b996-41eb-8811-462a3a4d436e")]
		#endregion
		[Indexed]
		[Type(typeof(PartInterface))]
		[Plural("ComponentParts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ComponentPart;



		public static PartBillOfMaterialInterface Instance {get; internal set;}

		internal PartBillOfMaterialInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}