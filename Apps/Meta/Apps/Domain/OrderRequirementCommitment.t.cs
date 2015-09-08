namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2fcdaf95-c3ec-4da2-8e7e-09c55741082f")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("OrderRequirementCommitments")]
	public partial class OrderRequirementCommitmentClass : Class
	{
		#region Allors
		[Id("a03b08be-82d9-4678-803a-0463c658d4c4")]
		[AssociationId("2ed48b3d-1c77-49f9-a970-836d066cc00f")]
		[RoleId("4f5be1db-964c-4c09-86ec-5b7bd06a4008")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Quantities")]
		public RelationType Quantity;

		#region Allors
		[Id("a9020377-d721-4329-868d-33ab63aed074")]
		[AssociationId("5654ce5d-3453-404c-86cb-dfc1cc175345")]
		[RoleId("85a19592-2e58-4d45-8463-2119658fa0b7")]
		#endregion
		[Indexed]
		[Type(typeof(OrderItemInterface))]
		[Plural("OrderItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType OrderItem;

		#region Allors
		[Id("e36224d2-cc6f-43b0-82e1-e300710f6407")]
		[AssociationId("5f56109c-0578-4db7-9c8a-de9617d374d8")]
		[RoleId("2f69978e-bd92-48b2-a711-58b4cf728d96")]
		#endregion
		[Indexed]
		[Type(typeof(RequirementInterface))]
		[Plural("Requirements")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Requirement;



		public static OrderRequirementCommitmentClass Instance {get; internal set;}

		internal OrderRequirementCommitmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}