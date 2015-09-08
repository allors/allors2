namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b0a42c94-3d4e-47f1-86a2-cf45eeba5f0d")]
	#endregion
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PositionResponsibilities")]
	public partial class PositionResponsibilityClass : Class
	{
		#region Allors
		[Id("493412a4-c29c-4e1c-9167-6c0c0dca831f")]
		[AssociationId("030fa5c5-e41f-4141-a91e-02b37a20e685")]
		[RoleId("fe87742c-4238-4be0-9f58-70ae3f01c96b")]
		#endregion
		[Indexed]
		[Type(typeof(PositionClass))]
		[Plural("Positions")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Position;

		#region Allors
		[Id("9c8794b9-2c7b-4afa-86a6-21fb48fc902f")]
		[AssociationId("7613dcb8-0c6f-4c65-96c0-75d2cc9db16e")]
		[RoleId("70d2a311-d09b-406c-89d4-3adbbc0a8fe2")]
		#endregion
		[Indexed]
		[Type(typeof(ResponsibilityClass))]
		[Plural("Responsibilities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Responsibility;



		public static PositionResponsibilityClass Instance {get; internal set;}

		internal PositionResponsibilityClass() : base(MetaPopulation.Instance)
        {
        }
	}
}